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
        if (PlayerBehaviorScript.Instance.getMovePoint().position == this.transform.position && !tipSeen) {
            dashTip.SetActive(true);
            tipSeen = true;
        }
    }
}
