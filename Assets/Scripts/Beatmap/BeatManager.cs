using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using MEC;


public class BeatManager : MonoBehaviour
{
    [SerializeField] private float  bpm;
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private Intervals[] intervals;

    [SerializeField] private AudioSource tickSound;

    [SerializeField] private GameObject indy;

    public static BeatManager Instance;

    protected Beatmap myMap;

    protected bool hasPlayedTick;
    protected bool songStarted;
    protected bool hitNote;
    protected bool readyForInput = false;
    protected bool noteExpired;

    protected float tolerance = 0.053f;
    protected float inputWindow = 0.2f; // in beats
    protected float inputLag = 0.45f;
    protected float universalOffset = 0f;

    protected int myCurrBeat=0;
    protected int numHitBeats=0;
    protected int numMissedBeats = 0;

    protected List<float> aboves;
    protected List<float> belows;

    void Awake() {
        if (Instance==null) {
            Instance = this;
            indy.SetActive(false);
            songStarted = false;
        }
    }

    private void Update() {
        //foreach(Intervals interval in intervals) {
        //    float sampledTime = (audioSource.timeSamples / (audioSource.clip.frequency * interval.GetIntervalLength(bpm)));
        //    interval.CheckForNewInterval(sampledTime);
        //}

        if (songStarted) {
            if (myCurrBeat<myMap.getHits().Length) {
                float nextHit = myMap.getHits()[myCurrBeat] +inputLag + universalOffset;
                float sampledTime = (audioSource.timeSamples / (audioSource.clip.frequency * Intervals.GetIntervalLength(bpm, 1f)));
                float diff = Mathf.Abs(nextHit-sampledTime);
                float unOffseted =  Mathf.Abs(myMap.getHits()[myCurrBeat]-sampledTime);
                if (!readyForInput && diff<=inputWindow) {
                    readyForInput = true;
                } 

                if (readyForInput && Input.GetKey(myMap.getKeys()[myCurrBeat])) {
                    Debug.Log("HIT!");
                    if (nextHit>=sampledTime) {
                        aboves.Add(diff);
                    } else {
                        belows.Add(-diff);

                    } 

                    hitNote=true;
                    numHitBeats++;
                }

                if (readyForInput && diff>inputWindow) {
                    noteExpired = true;
                    numMissedBeats++;
                    //Debug.Log("MISS! :(");
                }
         
                if (!hasPlayedTick &&  (unOffseted <= tolerance ||  sampledTime>=myMap.getHits()[myCurrBeat])) {
                    hasPlayedTick = true;
                    playTick( diff);
                }

                if (hitNote || noteExpired) {
                    myCurrBeat++;
                    hitNote = false;
                    noteExpired = false;
                    readyForInput = false;
                    hasPlayedTick = false;
                }
            } else {
                audioSource.Stop();
                songStarted = false;
                Debug.Log("SONG FINISHED");
                List<float> alls = new List<float>();
                alls.AddRange(aboves);
                alls.AddRange(belows);
                Debug.Log("Avg offset "  +  avg(alls));
                

                Debug.Log("Num Hit" + numHitBeats);
                Debug.Log("Num Missed" + numMissedBeats);
            }
            
        }
    }

    public float avg(List<float> listy) {
        int i=0;
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
    

    public void triggerOnBeats(Beatmap map) {
        audioSource.Play();
        myCurrBeat=0;
        myMap = map;
        belows = new List<float>();
        aboves = new List<float>();
        songStarted = true;
        
        //Timing.RunCoroutine(triggerOnBeatsCo(map).CancelWith(this.gameObject),Segment.Update,this.gameObject.GetInstanceID());
    }

    protected void playTick(float f) {
        tickSound.PlayOneShot(tickSound.clip);
        Timing.RunCoroutine(flashIndy().CancelWith(this.gameObject),this.gameObject.GetInstanceID());
    }

    
}

[System.Serializable]
public class Intervals {
    [SerializeField] private float durationInQuarterNotes;
    [SerializeField] private UnityEvent trigger;

    private int lastInterval;

    public float GetIntervalLength(float b) {
        return 60f/(b*(1/durationInQuarterNotes));
    }

    public static float GetIntervalLength(float b,float dur) {
        return 60f/(b*(1/dur));
    }

    public void CheckForNewInterval(float val) {
        if (Mathf.FloorToInt(val) != lastInterval) {
            lastInterval = Mathf.FloorToInt(val);
            trigger.Invoke();
        }
    }
}
