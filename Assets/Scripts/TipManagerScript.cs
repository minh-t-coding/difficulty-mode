using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

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
            Debug.Log("Update called");
            // Only display the tip if player has not beaten tutorial
            if (!seenTipNames.Contains(currentTip.name) && currentTip && !currentTip.name.Equals("StickoModeTip") && (PlayerPrefs.GetInt("levelAt") < 1)) {
                currentTip.SetActive(true);
                Time.timeScale = 0f;
            } else if (!seenTipNames.Contains(currentTip.name) && currentTip.name.Equals("UndoTip")) {
                currentTip.SetActive(true);
                Time.timeScale = 0f;
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

    public bool ShowStickoModeTooltip(GameObject tip) {
        if (!seenTipNames.Contains("StickoModeTooltip") && (PlayerPrefs.GetInt("levelAt") < 1)) {
            EnqueueTip(tip);
            return true;
        }
        return false;
    }

    public void EnqueueTip(GameObject newTip) {
        Debug.Log(newTip);
        tips.Enqueue(newTip);
    }

    public void DequeueTip() {
        if (tips.Count != 0) {
            GameObject closedTip = tips.Dequeue();
            seenTipNames.Add(closedTip.name);
        }
    }
}
