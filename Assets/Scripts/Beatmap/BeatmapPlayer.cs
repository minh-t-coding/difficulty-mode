using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MEC;

public class BeatmapPlayer : MonoBehaviour
{
    // Start is called before the first frame update

    protected int currNote;

    public void playBeatmap(Beatmap map) {
        currNote = 0;
        float[] hits = map.getHits();
        KeyCode[] keys = map.getKeys();

    }

    public IEnumerator<float> waitForInput(KeyCode k) {
        while(!Input.GetKeyDown(k)) {
            yield return Timing.WaitForOneFrame;
        }

    }

    public void onNote(float noteType) {
        
    }

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
