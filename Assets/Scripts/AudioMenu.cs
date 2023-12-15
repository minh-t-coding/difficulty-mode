using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class AudioMenu : MonoBehaviour
{
    public AudioMixer audioMixer;

    protected float currSfxVol;

    protected bool hasSfxVolChanged;
    void Start() {
        currSfxVol = 0f;
    }
    public void SetMusicVolume (float volume) {
        audioMixer.SetFloat("MusicParam", volume);
    }

    public void SetSFXVolume(float volume) {
        if (currSfxVol!=volume) {
            hasSfxVolChanged = true;
        }
        currSfxVol = volume;
        audioMixer.SetFloat("SFXParam", volume);
        
    }

    void Update() {
        if (Input.GetKeyUp(KeyCode.Mouse0) && hasSfxVolChanged) {
            hasSfxVolChanged = false;
            SoundManager.Instance.playSound("laser");
        }
    }

    public void SetMasterVolume(float volume) {
        audioMixer.SetFloat("MasterParam", volume);
    }
}
