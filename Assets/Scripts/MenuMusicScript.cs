using UnityEngine;
using System.Collections;

[RequireComponent(typeof(AudioSource))]
public class QueueAudioClip: MonoBehaviour
{
    public AudioSource audioSourceIntro;
    public AudioSource audioSourceLoop;
    private bool startedLoop;

    void FixedUpdate()
    {
        if (!audioSourceIntro.isPlaying && !startedLoop)
        {
            audioSourceLoop.Play();
            startedLoop = true;
        }
    }
}