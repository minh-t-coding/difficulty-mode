using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class AudioMenu : MonoBehaviour
{
    
    public AudioMixer audioMixer;

    [SerializeField] protected float gameStartVolume = 0.4f;

    protected float currSfxVol;

    protected bool hasSfxVolChanged;
    void Start() {
        currSfxVol = 0f;
    }

    void Awake() {
        audioMixer.SetFloat("MasterParam", Mathf.Log10(gameStartVolume) * 20);
        audioMixer.SetFloat("MusicParam", Mathf.Log10(gameStartVolume) * 20);
        audioMixer.SetFloat("SFXParam", Mathf.Log10(gameStartVolume) * 20);
    }
    public void SetMusicVolume (float volume) {
        audioMixer.SetFloat("MusicParam", Mathf.Log10(volume) * 20);
    }

    public void SetSFXVolume(float volume) {
        if (currSfxVol!=volume) {
            hasSfxVolChanged = true;
        }
        currSfxVol = volume;
        audioMixer.SetFloat("SFXParam", Mathf.Log10(volume) * 20);
        
    }

    public void SetMasterVolume(float volume) {
        audioMixer.SetFloat("MasterParam", Mathf.Log10(volume) * 20);
    }

    void Update() {
        if (Input.GetKeyUp(KeyCode.Mouse0) && hasSfxVolChanged) {
            hasSfxVolChanged = false;
            SoundManager.Instance.playSound("laser");
        }
    }

    
}
