using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using MEC;
using TMPro;


public class BeatManager : MonoBehaviour {
    private float bpm;
    private Intervals[] intervals;

    [SerializeField] private AudioSource tickSound;

    [SerializeField] private GameObject indy;

    [SerializeField] private GameObject hitPrefab;

    [SerializeField] private Transform mapParent;

    [SerializeField] private TMP_Text countdownTextBox;

    [SerializeField] private int numLives;

    [SerializeField] private GameObject bar;


    public static BeatManager Instance;

    protected Beatmap myMap;

    private AudioSource audioSource;

    protected bool hasPlayedTick;
    protected bool songStarted;
    protected bool hitNote;
    protected bool readyForInput = false;
    protected bool noteExpired;

    protected float tolerance = 0.053f;
    protected float inputWindow = 0.2f; // in beats
    protected float inputLag = 0.29f; // tuned to 130f
    protected float universalOffset = 0f;
    protected float mapScrollSpeed = 2f;

    protected float tunedInputLag;

    protected double startTimeOfOutro;

    protected int myCurrBeat = 0;
    protected int numHitBeats = 0;
    protected int numMissedBeats = 0;

    protected int countdown = 0;

    protected List<float> aboves;
    protected List<float> belows;


    protected Vector3 mapInitPos;

    protected List<BeatmapHit> beatmapHits;

    protected KeyCode[] keyOrder = new KeyCode[6] { KeyCode.A, KeyCode.W, KeyCode.S, KeyCode.D, KeyCode.Space, KeyCode.Return };

    void Awake() {
        if (Instance == null) {
            Instance = this;
            indy.SetActive(false);
            bar.SetActive(false);
            songStarted = false;
            mapInitPos = mapParent.transform.localPosition;
            countdownTextBox.gameObject.SetActive(false);
        }
    }

    public IEnumerator<float> CountDownAnim() {
        while (AudioSettings.dspTime < startTimeOfOutro) {
            yield return Timing.WaitForSeconds(0f);
        }
        countdownTextBox.gameObject.SetActive(true);
        for (int i = 0; i < 5; i++) {
            while (AudioSettings.dspTime < (startTimeOfOutro + ((60.0f / bpm) * (i)))) {
                yield return Timing.WaitForSeconds(0f);
            }
            countdownTextBox.text = (4 - i).ToString();
        }
        countdownTextBox.gameObject.SetActive(false);
    }

    public IEnumerator<float> FinishAnim() {

        yield return Timing.WaitForSeconds(0.1f);
        GameStateManager.Instance.loadGameState(currState, false);
         yield return Timing.WaitForSeconds(0.9f);
        float  initVol = audioSource.volume;
        int numSteps = 40;
        for(int i=0;i<numSteps;i++) {
            audioSource.volume -= initVol/numSteps;
            yield return Timing.WaitForSeconds(0.1f);
        }
        audioSource.Stop();
        audioSource.volume = initVol;
    }

    public IEnumerator<float> FailAnim() {

        float initVol = audioSource.volume;
        float initPitch = audioSource.pitch;
        int numSteps = 10;
        SoundManager.Instance.playSound("recordScratch");
        for(int i=0;i<numSteps;i++) {
            audioSource.volume -= initVol/numSteps;
            audioSource.pitch -= initPitch/numSteps;
            yield return Timing.WaitForSeconds(0.1f);
        }
        audioSource.Stop();
        audioSource.volume = initVol;
        audioSource.pitch = initPitch;
    }

    public void constructBeatmap(Beatmap map) {
        float[] hits = map.getHits();
        KeyCode[] keys = map.getKeys();
        beatmapHits = new List<BeatmapHit>();
        int i = 0;
        mapParent.transform.localPosition = mapInitPos + new Vector3(0, -(-myMap.getDelay() - tunedInputLag) * (mapScrollSpeed * transform.lossyScale.y), 0);
        foreach (float hit in hits) {
            GameObject newHit = Instantiate(hitPrefab);
            newHit.transform.SetParent(mapParent);
            newHit.transform.localPosition = new Vector3(getHitHoriPos(keys[i]), hit * mapScrollSpeed, 0f);
            Vector3 initScale = newHit.transform.localScale;
            newHit.transform.localScale = new Vector3(transform.lossyScale.x * initScale.x, transform.lossyScale.y * initScale.y, transform.lossyScale.z * initScale.z);
            BeatmapHit hitComp = newHit.GetComponent<BeatmapHit>();
            hitComp.setKey(keys[i].ToString());
            beatmapHits.Add(hitComp);
            i++;
        }
        mapParent.gameObject.SetActive(false);

    }

    protected float getHitHoriPos(KeyCode key) {
        float spacing = 1f;
        float offset = -4f;
        for (int i = 0; i < keyOrder.Length; i++) {
            if (keyOrder[i] == key) {
                return offset + spacing * i;
            }
        }

        return offset - spacing;

    }

    public int getConcurrentHits(Beatmap map, int curr) {
        float[] hits = map.getHits();
        List<KeyCode> concurrentHits = new List<KeyCode>();
        KeyCode[] keys = map.getKeys();
        float currBeat = hits[curr];
        for (int i = curr; i < hits.Length; i++) {
            if (hits[i] == hits[curr]) {
                concurrentHits.Add(keys[i]);
            }
            else {
                break;
            }
        }
        return concurrentHits.Count;

    }

    
    protected bool hasLoadedFirstState = false;
    int currState = 1;

    protected bool hasFailed = false;
    private void Update() {

        //foreach(Intervals interval in intervals) {
        //    float sampledTime = (audioSource.timeSamples / (audioSource.clip.frequency * interval.GetIntervalLength(bpm)));
        //    interval.CheckForNewInterval(sampledTime);
        //}

        if (songStarted && !hasFailed && audioSource != null && AudioSettings.dspTime > startTimeOfOutro && audioSource.isPlaying) {
            if (numMissedBeats >= numLives) {
                PlayerInputManager.Instance.setAllowedActions(new List<KeyCode>(), false);
                Debug.Log("you lose >:(, restart nerd");
                //audioSource.Stop();
                hasFailed = true;
                Timing.RunCoroutine(FailAnim().CancelWith(gameObject), this.gameObject.GetInstanceID());
                return;
            }
            if (!hasLoadedFirstState) {
                hasLoadedFirstState = true;
                GameStateManager.Instance.loadGameState(currState, false);
                currState++;
            }
            mapParent.gameObject.SetActive(true);
            if (myCurrBeat < myMap.getHits().Length) {
                float nextHit = myMap.getHits()[myCurrBeat] + tunedInputLag + universalOffset;
                float sampledTime = (audioSource.timeSamples / (audioSource.clip.frequency * Intervals.GetIntervalLength(bpm, 1f))) - myMap.getDelay();
                mapParent.transform.localPosition = mapInitPos + new Vector3(0, -(sampledTime - tunedInputLag) * (mapScrollSpeed), 0);
                float diff = Mathf.Abs(nextHit - sampledTime);
                float unOffseted = Mathf.Abs(myMap.getHits()[myCurrBeat] - sampledTime);
                int numHits = getConcurrentHits(myMap, myCurrBeat);
                List<KeyCode> concurrentHits = new List<KeyCode>();

                for (int i = 0; i < numHits; i++) {
                    concurrentHits.Add(myMap.getKeys()[myCurrBeat + i]);
                }
                if (!readyForInput && diff <= inputWindow) {
                    readyForInput = true;
                    if (myCurrBeat > 0) {
                        if (myMap.getHits().Length > myCurrBeat + numHits) { // hacky way to handle dashes
                            float currNote = myMap.getHits()[myCurrBeat];
                            float noteType = currNote - Mathf.Floor(currNote);
                            float distToNextNote = myMap.getHits()[myCurrBeat + numHits] - myMap.getHits()[myCurrBeat];
                            if (noteType == 0.0f || distToNextNote != 0.5f) {
                                GameStateManager.Instance.loadGameState(currState, false);
                                currState++;
                            }
                        }
                        else {
                            GameStateManager.Instance.loadGameState(currState, false);
                            currState++;
                        }
                    }
                    PlayerInputManager.Instance.setAllowedActions(concurrentHits, myMap.getActions()[myCurrBeat] == PlayerState.PlayerAction.Dash);
                }
                if (readyForInput && numHits > 0) {
                    bool pressedAllHits = true;
                    foreach (KeyCode key in concurrentHits) {
                        pressedAllHits = pressedAllHits && Input.GetKey(key);
                    }

                    if (pressedAllHits) {

                        Debug.Log("HIT!");
                        if (nextHit >= sampledTime) {
                            aboves.Add(diff);
                        }
                        else {
                            belows.Add(-diff);

                        }
                        for (int i = 0; i < numHits; i++) {
                            beatmapHits[myCurrBeat + i].CreateHitEffect();
                        }

                        hitNote = true;
                        numHitBeats++;
                    }
                }

                if (diff > inputWindow) {
                    if (readyForInput) {
                        noteExpired = true;
                        for (int i = 0; i < numHits; i++) {
                            beatmapHits[myCurrBeat + i].CreateMissEffect();
                        }
                        numMissedBeats++;

                        Debug.Log("LOAD STATE" + currState);
                    }


                    //Debug.Log("MISS! :(");
                }

                if (!hasPlayedTick && (unOffseted <= tolerance || sampledTime >= myMap.getHits()[myCurrBeat])) {
                    hasPlayedTick = true;
                    //playTick( diff);
                }

                if (hitNote || noteExpired) {



                    myCurrBeat += numHits;
                    hitNote = false;
                    noteExpired = false;
                    readyForInput = false;
                    hasPlayedTick = false;

                }
            }
            else {
                PlayerInputManager.Instance.setAllowedActions(new List<KeyCode>(), false);
                songStarted = false;
                Debug.Log("SONG FINISHED");
                List<float> alls = new List<float>();
                alls.AddRange(aboves);
                alls.AddRange(belows);
                Debug.Log("Avg offset " + avg(alls));
                Timing.RunCoroutine(FinishAnim().CancelWith(gameObject), this.gameObject.GetInstanceID());
                
                Debug.Log("Num Hit" + numHitBeats);
                Debug.Log("Num Missed" + numMissedBeats);
                Debug.Log("Yay you win :)");

                // Progress level on beatmap win
                LevelHandlerScript.Instance.progressLevel();
            }

        }
    }

    public float avg(List<float> listy) {
        int i = 0;
        float sum = 0;
        for (i = 0; i < listy.Count; i++) {
            sum += listy[i];
        }
        return sum / listy.Count;
    }

    protected IEnumerator<float> flashIndy() {
        indy.SetActive(true);
        yield return Timing.WaitForSeconds(0.1f);
        indy.SetActive(false);
    }


    public void triggerBeatmap(Beatmap map, SongObj song, AudioSource source, double startTime) {
        bar.SetActive(true);
        hasFailed = false;
        PlayerInputManager.Instance.setIsStickoMode(true);
        audioSource = source;
        startTimeOfOutro = startTime;
        bpm = song.getBpm();
        myMap = map;
        constructBeatmap(map);
        //audioSource.Play();
        myCurrBeat = 0;
        countdown = 0;
        tunedInputLag = (bpm / 130f) * inputLag;
        belows = new List<float>();
        aboves = new List<float>();
        songStarted = true;

        Timing.RunCoroutine(CountDownAnim().CancelWith(gameObject), this.gameObject.GetInstanceID());
        //Timing.RunCoroutine(triggerOnBeatsCo(map).CancelWith(this.gameObject),Segment.Update,this.gameObject.GetInstanceID());
    }

    protected void playTick(float f) {
        tickSound.PlayOneShot(tickSound.clip);
        Timing.RunCoroutine(flashIndy().CancelWith(this.gameObject), this.gameObject.GetInstanceID());
    }


}

[System.Serializable]
public class Intervals {
    [SerializeField] private float durationInQuarterNotes;
    [SerializeField] private UnityEvent trigger;

    private int lastInterval;

    public float GetIntervalLength(float b) {
        return 60f / (b * (1 / durationInQuarterNotes));
    }

    public static float GetIntervalLength(float b, float dur) {
        return 60f / (b * (1 / dur));
    }

    public void CheckForNewInterval(float val) {
        if (Mathf.FloorToInt(val) != lastInterval) {
            lastInterval = Mathf.FloorToInt(val);
            trigger.Invoke();
        }
    }
}
