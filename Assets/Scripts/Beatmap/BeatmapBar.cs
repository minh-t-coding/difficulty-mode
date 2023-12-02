using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeatmapBar : MonoBehaviour
{
    // Start is called before the first frame update
    public static BeatmapBar Instance;
    void Awake() {
        if (Instance==null) {
            Instance = this;
        }
    }
}
