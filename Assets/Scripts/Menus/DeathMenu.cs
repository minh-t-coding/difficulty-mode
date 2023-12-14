using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DeathMenu : MonoBehaviour
{
    public static DeathMenu Instance;
    
    [SerializeField] protected GameObject deathMenuUI;

    [SerializeField] protected GameObject winMenuUI;

    void Awake() {
        if (Instance == null) {
            Instance = this;
        }
    }
    
    // Update is called once per frame
    void Update()
    {

    }

    public void Resume() {
        deathMenuUI.SetActive(false);
        Time.timeScale = 1f;
    }

    public void ShowDeathMenu() {
        deathMenuUI.SetActive(true);
        //Time.timeScale = 0f;
    }

    public void ShowWinMenu() {
        winMenuUI.SetActive(true);
        //Time.timeScale = 0f;
    }

    public void LoadMenu() {
        Resume();
        SceneManager.LoadScene(0);
    }

    public void Restart() {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        Resume();
    }
}
