using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MEC;
public class SongTransitioner : MonoBehaviour
{
    // Start is called before the first frame update

    
    public static IEnumerator<float> transitionLoopIntoSong(AudioSource songToStop, AudioSource songToPlay, Beatmap bm) {
        songToStop.loop = false;
        while(songToStop.isPlaying) {
            yield return 0f;
        }
        songToPlay.Play();
        BeatManager.Instance.triggerOnBeats(bm);
    }
}
