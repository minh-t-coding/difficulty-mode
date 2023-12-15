using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(menuName = "Song/SongObj")]
public class SongObj : ScriptableObject
{
    [SerializeField] protected float bpm;

    [SerializeField] protected bool isHalfTime;
    [SerializeField] protected AudioClip introPart;

    [SerializeField] protected float introLength;

    [SerializeField] protected float introVolume = 1f;

    [SerializeField] protected AudioClip loopingPart;

    [SerializeField] protected float loopingLength;

    [SerializeField] protected float loopingVolume = 1f;

    [SerializeField] protected AudioClip outroPart;

    [SerializeField] protected float outroLength;

    [SerializeField] protected float outroVolume = 1f;

    [SerializeField] protected AudioClip outroLoopPart;

    [SerializeField] protected float outroLoopLength;

    [SerializeField] protected float outroLoopVolume = 1f;

    public float getBpm() {
        return bpm;
    }

    public bool getIsHalfTime() {
        return isHalfTime;
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

    public float getOutroVolume() {
        return outroVolume;
    }

    public float getIntroVolume() {
        return introVolume;
    }

    public float getLoopingVolume() {
        return loopingVolume;
    }

    public AudioClip getOutroPart() {
        return outroPart;
    }

    public float getOutroLength() {
        return outroLength;
    }

     public float getOutroLoopLength() {
        return outroLoopLength;
    }

    public AudioClip getOutroLoopPart() {
        return outroLoopPart;
    }

    public float getOutroLoopVolume() {
        return outroLoopVolume;
    }
}
