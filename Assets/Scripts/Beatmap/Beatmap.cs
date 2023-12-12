using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Beatmap
{
    protected float[] hits;

    protected KeyCode[] keys;

    protected PlayerState.PlayerAction[] actions;

    protected float delay;

    

    public Beatmap(float[] h, KeyCode[] k, PlayerState.PlayerAction[] a,  float d = 0f ) {
        hits = h;
        keys = k;
        actions = a;
        delay = d;
    }

    public float[] getHits() {
        return hits;
    }

    public PlayerState.PlayerAction[] getActions() {
        return actions;
    }

    public KeyCode[] getKeys() {
        return keys;
    }

    public float getDelay() {
        return delay;
    }


}
