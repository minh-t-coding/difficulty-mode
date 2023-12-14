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

    protected Beatmap myBeatmap;

    protected bool hasStarted;
    void init()
    {
        hasStarted=true;
        //myBeatmap = new Beatmap(hits, keys,delay);
        //SongTransitionerController.Instance.startTransition(myBeatmap);
        
        SongTransitionerController.Instance.PlaySongImmediately();
    }

    void Update() {
        if (!hasStarted && Input.GetKey(KeyCode.Return)) {
            init();
        }
    }


}
