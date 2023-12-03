using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Beatmap
{
    protected float[] hits;

    protected KeyCode[] keys;

    protected float delay;

    

    public Beatmap(float[] h, KeyCode[] k, float d = 0f ) {
        hits = h;
        keys = k;
        delay = d;
    }

    public float[] getHits() {
        return hits;
    }

    public KeyCode[] getKeys() {
        return keys;
    }

    public float getDelay() {
        return delay;
    }


}
