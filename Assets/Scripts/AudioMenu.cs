using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class AudioMenu : MonoBehaviour
{
    public AudioMixer audioMixer;
    public void SetMusicVolume (float volume) {
        audioMixer.SetFloat("MusicParam", volume);
    }

    public void SetSFXVolume(float volume) {
        audioMixer.SetFloat("SFXParam", volume);
    }

    public void SetMasterVolume(float volume) {
        audioMixer.SetFloat("MasterParam", volume);
    }
}
