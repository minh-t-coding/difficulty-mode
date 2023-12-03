using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MEC;
public class BeatmapTester : MonoBehaviour
{
    [SerializeField] protected float[] hits;

    [SerializeField] protected KeyCode[] keys;

    [SerializeField] protected float delay;

    [SerializeField] protected GameObject text;

    [SerializeField] protected AudioSource loop;

    [SerializeField] protected AudioSource song;
    protected Beatmap myBeatmap;

    protected bool hasStarted;
    void init()
    {
        hasStarted=true;
        myBeatmap = new Beatmap(hits, keys,delay);
        Timing.RunCoroutine(SongTransitioner.transitionLoopIntoSong(loop, song, myBeatmap).CancelWith(gameObject),this.gameObject.GetInstanceID());
        text.SetActive(false);
    }

    void Update() {
        if (!hasStarted && Input.GetKey(KeyCode.Return)) {
            init();
        }
    }


}
