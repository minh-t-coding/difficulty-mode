using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(menuName = "Song/SongObj")]
public class SongObj : ScriptableObject
{
    [SerializeField] protected float bpm;
    [SerializeField] protected AudioClip introPart;

    [SerializeField] protected float introLength;

    [SerializeField] protected AudioClip loopingPart;

    [SerializeField] protected float loopingLength;

    [SerializeField] protected AudioClip outroPart;

    public float getBpm() {
        return bpm;
    }

    public AudioClip getIntroPart() {
        return introPart;
    }

    public float getIntroLength() {
        return introLength;
    }

    public AudioClip getLoopingPart() {
        return loopingPart;
    }

    public float getLoopingLength() {
        return loopingLength;
    }

    public AudioClip getOutroPart() {
        return outroPart;
    }
}
