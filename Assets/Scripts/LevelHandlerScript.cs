using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelHandlerScript : MonoBehaviour {
    public static LevelHandlerScript Instance;
    private int nextSceneLoad;

    void Awake() {
        if (Instance == null) {
            Instance = this;
        }
    }

    void Start() {
        nextSceneLoad = SceneManager.GetActiveScene().buildIndex + 1;
    }

    public void progressLevel() {
        if (SceneManager.GetActiveScene().buildIndex == 4) { // On the last level
            Debug.Log("YOU WIN THE GAME!");
        } else {
            // Move to next level
            SceneManager.LoadScene(nextSceneLoad);

            if (nextSceneLoad > PlayerPrefs.GetInt("levelAt")) {
                PlayerPrefs.SetInt("levelAt", nextSceneLoad);
            }
        }
    }
}
