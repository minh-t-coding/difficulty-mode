using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TipManagerScript : MonoBehaviour
{
    public static TipManagerScript Instance;
    private Dictionary<string, GameObject> allTips;
    private Queue<GameObject> tips;
    private HashSet<string> seenTipNames;

    void Awake() {
        if (Instance == null) {
            Instance = this;
        }
        tips = new Queue<GameObject>();
        allTips = new Dictionary<string, GameObject>();
        seenTipNames = new HashSet<string>();
    }
    void Update() {
        if (tips.Count != 0) {
            GameObject currentTip = tips.Peek();
            if (!seenTipNames.Contains(currentTip.name)) {
                currentTip.SetActive(true);
            }
        }
    }

    public void addToAllTips(string name, GameObject tip) {
        if (!allTips.ContainsKey(name)) {
            allTips.Add(name, tip);
        }
    }

    public GameObject GetTip(string name) {
        if (allTips.ContainsKey(name)) {
            return allTips[name];
        }
        return null;
    }

    public void EnqueueTip(GameObject newTip) {
        tips.Enqueue(newTip);
    }

    public void DequeueTip() {
        if (tips.Count != 0) {
            GameObject closedTip = tips.Dequeue();
            seenTipNames.Add(closedTip.name);
        }
    }
}
