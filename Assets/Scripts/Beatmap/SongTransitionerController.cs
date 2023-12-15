using System;
using UnityEngine;
using System.Collections;

// Basic demonstration of a music system that uses PlayScheduled to preload and sample-accurately
// stitch two AudioClips in an alternating fashion.  The code assumes that the music pieces are
// each 16 bars (4 beats / bar) at a tempo of 140 beats per minute.
// To make it stitch arbitrary clips just replace the line
//   nextEventTime += (60.0 / bpm) * numBeatsPerSegment
// by
//   nextEventTime += clips[flip].length;


public class SongTransitionerController : MonoBehaviour {

    public static SongTransitionerController Instance;

    [SerializeField] private AudioSource[] loopSources;

    [SerializeField] protected SongObj currSong;

    [SerializeField] protected bool playSongOnStart = true;



    private int flip = 0;


    private bool running = false;

    protected bool beginTransition;

    protected float currSegmentLength;

    protected bool hasPlayedIntro;

    protected double lastEventTime = 0f;

    protected double nextEventTime;

    public float getBpmSpeedMultiplier() {
        float currBpm = currSong.getBpm();
        float diff = currBpm - 77.5f;
        return 1+diff/232.5f;
    }

    void Awake() {
        if (Instance == null) {
            Instance = this;
        }
    }

    public void startTransition(Beatmap bm) {
        beginTransition = true;
        running = false;
        double barLength = (60.0f / currSong.getBpm()) * (currSong.getIsHalfTime()? 2 : 4);
        if (lastEventTime <= AudioSettings.dspTime) {
            double timeIntoSegment = AudioSettings.dspTime - lastEventTime;
            double barsIntoSegment = timeIntoSegment / barLength;
            double timeToNextBar = (Mathf.CeilToInt((float)barsIntoSegment) - barsIntoSegment) * barLength;
            double targTime = lastEventTime + timeIntoSegment + timeToNextBar;
            loopSources[flip].clip = currSong.getOutroPart();
            loopSources[flip].PlayScheduled(targTime);
            loopSources[1 - flip].SetScheduledEndTime(targTime);
            loopSources[flip].volume = currSong.getOutroVolume();
            BeatManager.Instance.triggerBeatmap(bm, currSong, loopSources[flip], targTime);
        }
        else {
            double targTime = lastEventTime;
            loopSources[flip].clip = currSong.getOutroPart();
            loopSources[flip].PlayScheduled(targTime);
            loopSources[flip].volume = currSong.getOutroVolume();
            loopSources[1 - flip].SetScheduledEndTime(targTime);
            BeatManager.Instance.triggerBeatmap(bm, currSong, loopSources[flip], targTime);
        }

    }

    void Start() {
        if (playSongOnStart) {
            PlaySong(currSong);
        } 
    }

    public void PlaySongImmediately() {

        hasPlayedIntro = true;
        nextEventTime = AudioSettings.dspTime+0.05f;
        loopSources[flip].clip = currSong.getIntroPart();
        loopSources[flip].PlayScheduled(nextEventTime);
        loopSources[flip].volume = currSong.getIntroVolume();
        currSegmentLength = currSong.getIntroLength();
        running = true;
        nextEventTime += 60.0f / currSong.getBpm() * (currSegmentLength);
        flip = 1 - flip;
    }

    public void PlaySong(SongObj s) {
        currSong = s;
        nextEventTime = AudioSettings.dspTime + 2f;
        hasPlayedIntro = false;
        beginTransition = false;
        running = true;
    }


    void Update() {
        if (!running) {
            return;
        }

        double time = AudioSettings.dspTime;

        if (time + 1f >= nextEventTime) {
            if (!hasPlayedIntro) {
                hasPlayedIntro = true;
                loopSources[flip].clip = currSong.getIntroPart();
                loopSources[flip].volume = currSong.getIntroVolume();
                loopSources[flip].PlayScheduled(nextEventTime);
                currSegmentLength = currSong.getIntroLength();
            }
            else {
                loopSources[flip].clip = currSong.getLoopingPart();
                loopSources[flip].volume = currSong.getLoopingVolume();
                loopSources[flip].PlayScheduled(nextEventTime);
                currSegmentLength = currSong.getLoopingLength();
            }
            Debug.Log("Scheduled source " + flip + " to start at time " + nextEventTime);
            lastEventTime = nextEventTime;
            nextEventTime += 60.0f / currSong.getBpm() * (currSegmentLength);
            flip = 1 - flip;
        }
    }

    public SongObj getCurrSong() {
        return currSong;
    }
}