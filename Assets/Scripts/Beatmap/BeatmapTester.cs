using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeatmapTester : MonoBehaviour
{
    [SerializeField] protected float[] hits;

    [SerializeField] protected KeyCode[] keys;

    [SerializeField] protected GameObject text;
    protected Beatmap myBeatmap;

    protected bool hasStarted;
    void init()
    {
        hasStarted=true;
        myBeatmap = new Beatmap(hits, keys);
        BeatManager.Instance.triggerOnBeats(myBeatmap);
        text.SetActive(false);
    }

    void Update() {
        if (!hasStarted && Input.GetKey(KeyCode.Return)) {
            init();
        }
    }


}
