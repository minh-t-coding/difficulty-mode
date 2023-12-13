using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TipManagerScript : MonoBehaviour
{
    public static TipManagerScript Instance;
    private Queue<GameObject> tips;

    void Awake() {
        if (Instance == null) {
            Instance = this;
        }
        tips = new Queue<GameObject>();
    }
    void Update() {
        if (tips.Count != 0) {
            GameObject currentTip = tips.Peek();
            currentTip.SetActive(true);
        }
    }

    public void EnqueueTip(GameObject newTip) {
        tips.Enqueue(newTip);
    }

    public void DequeueTip() {
        if (tips.Count != 0) {
            tips.Dequeue();
        }
    }
}
