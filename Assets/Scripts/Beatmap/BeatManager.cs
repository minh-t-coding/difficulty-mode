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

    [SerializeField] private GameObject hitPrefab;

    [SerializeField] private Transform mapParent;

    public static BeatManager Instance;

    protected Beatmap myMap;

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

    protected int myCurrBeat=0;
    protected int numHitBeats=0;
    protected int numMissedBeats = 0;

    protected List<float> aboves;
    protected List<float> belows;
    

    protected Vector3 mapInitPos;

    protected List<BeatmapHit> beatmapHits;

    protected KeyCode[] keyOrder = new KeyCode[6] {KeyCode.A,KeyCode.W ,KeyCode.S ,KeyCode.D, KeyCode.Space, KeyCode.Return  };

    void Awake() {
        if (Instance==null) {
            Instance = this;
            indy.SetActive(false);
            songStarted = false;
            mapInitPos = mapParent.transform.position;
        }
    }

    public void constructBeatmap(Beatmap map) {
        float[] hits = map.getHits();
        KeyCode[] keys = map.getKeys();
        beatmapHits = new List<BeatmapHit>();
        int i=0;
         mapParent.transform.position = mapInitPos + new Vector3(0,-(-myMap.getDelay()-tunedInputLag)*mapScrollSpeed,0) ;
        foreach(float hit in hits) {
            GameObject newHit = Instantiate(hitPrefab);
            newHit.transform.SetParent(mapParent);
            newHit.transform.localPosition = new Vector3 (getHitHoriPos(keys[i]),hit*mapScrollSpeed,0f);
            BeatmapHit hitComp = newHit.GetComponent<BeatmapHit>();
            hitComp.setKey(keys[i].ToString());
            beatmapHits.Add(hitComp);
            i++;
        }
       
    }

    protected float getHitHoriPos(KeyCode key) {
        float spacing = 1f;
        float offset = -4f;
        for (int i=0;i<keyOrder.Length;i++) {
            if (keyOrder[i] == key) {
                return offset+spacing*i;
            }
        }

        return offset-spacing;

    }

    public int getConcurrentHits(Beatmap map,int curr) {
        float[] hits = map.getHits();
        List<KeyCode> concurrentHits = new List<KeyCode>();
        KeyCode[] keys = map.getKeys();
        float currBeat = hits[curr];
        for(int i=curr;i<hits.Length;i++) {
            if (hits[i]==hits[curr]) {
                concurrentHits.Add(keys[i]);
            } else {
                break;
            }
        }
        return concurrentHits.Count;

    }

    private void Update() {
        //foreach(Intervals interval in intervals) {
        //    float sampledTime = (audioSource.timeSamples / (audioSource.clip.frequency * interval.GetIntervalLength(bpm)));
        //    interval.CheckForNewInterval(sampledTime);
        //}

        if (songStarted) {
            if (myCurrBeat<myMap.getHits().Length) {
                float nextHit = myMap.getHits()[myCurrBeat] +tunedInputLag + universalOffset;
                float sampledTime = (audioSource.timeSamples / (audioSource.clip.frequency * Intervals.GetIntervalLength(bpm, 1f))) - myMap.getDelay();
                mapParent.transform.position = mapInitPos + new Vector3(0,-(sampledTime-tunedInputLag)*mapScrollSpeed,0) ;
                float diff = Mathf.Abs(nextHit-sampledTime);
                float unOffseted =  Mathf.Abs(myMap.getHits()[myCurrBeat]-sampledTime);
                int numHits = getConcurrentHits(myMap,myCurrBeat);
                if (!readyForInput && diff<=inputWindow) {
                    readyForInput = true;
                } 
                if (readyForInput &&  numHits>0) {
                    bool pressedAllHits = true;
                    for (int i=0;i<numHits;i++) {
                        pressedAllHits = pressedAllHits && Input.GetKey(myMap.getKeys()[myCurrBeat+i]);
                    }
                    
                    if (pressedAllHits) {
                        Debug.Log("HIT!");
                        if (nextHit>=sampledTime) {
                            aboves.Add(diff);
                        } else {
                            belows.Add(-diff);

                        } 
                        for (int i=0;i<numHits;i++) {
                            beatmapHits[myCurrBeat+i].CreateHitEffect();
                        }
                        
                        hitNote=true;
                        numHitBeats++;
                    }
                }

                if (readyForInput && diff>inputWindow) {
                    noteExpired = true;
                    for (int i=0;i<numHits;i++) {
                        beatmapHits[myCurrBeat+i].CreateMissEffect();
                    }
                    numMissedBeats++;
                    //Debug.Log("MISS! :(");
                }
         
                if (!hasPlayedTick &&  (unOffseted <= tolerance ||  sampledTime>=myMap.getHits()[myCurrBeat])) {
                    hasPlayedTick = true;
                    //playTick( diff);
                }
 
                if (hitNote || noteExpired) {
                    myCurrBeat+=numHits;
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
        myMap = map;
        constructBeatmap(map);
        //audioSource.Play();
        myCurrBeat=0;
        tunedInputLag = (bpm/130f) * inputLag;
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
