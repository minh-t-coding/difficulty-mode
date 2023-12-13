using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelButtonsHandler : MonoBehaviour
{
    [SerializeField] Button[] levelButtons;

    private void Start() {
        int levelAt = PlayerPrefs.GetInt("levelAt", 1);

        for (int i = 0; i < levelButtons.Length; i++) {
            if (i + 1 > levelAt) {
                levelButtons[i].interactable = false;
            }
        }
    }

    public void UpdateAccessibleLevels() {
        int levelAt = PlayerPrefs.GetInt("levelAt", 1);

        for (int i = 0; i < levelButtons.Length; i++) {
            if (i + 1 > levelAt) {
                levelButtons[i].interactable = false;
            }
        }
    }
}
