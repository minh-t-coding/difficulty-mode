using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Beatmap
{
    protected float[] hits;

    protected KeyCode[] keys;

    public Beatmap(float[] h, KeyCode[] k ) {
        hits = h;
        keys = k;

    }

    public float[] getHits() {
        return hits;
    }

    public KeyCode[] getKeys() {
        return keys;
    }


}
