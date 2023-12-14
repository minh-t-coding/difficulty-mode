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
}
