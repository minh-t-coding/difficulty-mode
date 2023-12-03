using UnityEngine;
using System.Collections;

// Basic demonstration of a music system that uses PlayScheduled to preload and sample-accurately
// stitch two AudioClips in an alternating fashion.  The code assumes that the music pieces are
// each 16 bars (4 beats / bar) at a tempo of 140 beats per minute.
// To make it stitch arbitrary clips just replace the line
//   nextEventTime += (60.0 / bpm) * numBeatsPerSegment
// by
//   nextEventTime += clips[flip].length;


public class SongTransitionerController : MonoBehaviour
{
    public float bpm = 130;
    public int numBeatsPerSegment = 16;
    
    public static SongTransitionerController Instance;

    void Awake() {
        if (Instance==null) {
            Instance = this;
        }
    }
    private double nextEventTime;
    private int flip = 0;
    [SerializeField] private AudioSource[] loopSources;

    [SerializeField] private AudioSource mainSource;

    [SerializeField] private AudioClip[] clips = new AudioClip[2];
    private bool running = false;

    protected bool beginTransition;

    public void startTransition() {
        beginTransition = true;
    }

    void Start()
    {
        nextEventTime = AudioSettings.dspTime + 2.0f;
        running = true;
    }

    void Update()
    {
        if (!running)
        {
            return;
        }

        double time = AudioSettings.dspTime;

        if (time + 1.0f >= nextEventTime)
        {
            if (beginTransition) {
                mainSource.PlayScheduled(nextEventTime);
                running = false;
            } else {
                loopSources[flip].clip = clips[flip];
                loopSources[flip].PlayScheduled(nextEventTime);
            }

            Debug.Log("Scheduled source " + flip + " to start at time " + nextEventTime);
            nextEventTime += 60.0f / bpm * (numBeatsPerSegment);
            flip = 1 - flip;
        }
    }
}