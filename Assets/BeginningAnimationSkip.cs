using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeginningAnimationSkip : MonoBehaviour
{
    [SerializeField] protected GameObject mainMenu;
    [SerializeField] protected GameObject background;

    void Awake() {
        Time.timeScale = 1f;
    }
    
    void Update() {
        if (Input.anyKey && (PlayerPrefs.GetInt("hasSeenIntro", 0) == 1)) {
            this.gameObject.SetActive(false);
            mainMenu.SetActive(true);
            background.SetActive(true);
        }
    }
}
