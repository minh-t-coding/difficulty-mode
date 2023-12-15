using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;
public class DeathMenu : MonoBehaviour
{
    public static DeathMenu Instance;
    
    [SerializeField] protected GameObject deathMenuUI;

    [SerializeField] protected GameObject winMenuUI;

    [SerializeField] protected Button stickoModeRestartButton;

    [SerializeField] protected TextMeshProUGUI attemptsRemanining;

    [SerializeField] protected int stickoModeAttempts;

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

    public void renderDeathMenu() {
        if (stickoModeAttempts == -1) {
            attemptsRemanining.text = "";
            stickoModeRestartButton.interactable = true;
            return;
        }
        attemptsRemanining.text = "attempts remaining: " + (stickoModeAttempts-BeatManager.Instance.getNumStickoModeAttempts());
        if (stickoModeAttempts>BeatManager.Instance.getNumStickoModeAttempts()) {
            stickoModeRestartButton.interactable = true;
        } else {
            stickoModeRestartButton.interactable = false;
        }
    }

    public void ShowDeathMenu() {
        renderDeathMenu();
        deathMenuUI.SetActive(true);
        //Time.timeScale = 0f;
    }

    public void HideDeathMenu() {
        renderDeathMenu();
        deathMenuUI.SetActive(false);
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
