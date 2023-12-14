using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DashTipTriggerScript : MonoBehaviour {
    private GameObject dashTip;
    private bool tipSeen = false;
    void Awake() {
        dashTip = GameObject.Find("DashTip");
        if (dashTip != null) {
            dashTip.SetActive(false);
        }
    }

    void Update() {
        Vector3 extraTile = this.transform.position + Vector3.up;
        bool isOnTriggerTile = PlayerBehaviorScript.Instance.getPlayerTransform().position == this.transform.position || PlayerBehaviorScript.Instance.getPlayerTransform().position == extraTile;
        if (isOnTriggerTile && !tipSeen) {
            TipManagerScript.Instance.EnqueueTip(dashTip);
            tipSeen = true;
        }
    }
}
