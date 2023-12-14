using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialTipScript : MonoBehaviour
{
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.T)) {
            TipManagerScript.Instance.DequeueTip();
            this.gameObject.SetActive(false);
        }
    }
}
