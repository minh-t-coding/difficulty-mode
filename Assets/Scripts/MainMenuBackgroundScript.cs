using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuBackgroundScript : MonoBehaviour
{
    [SerializeField] protected GameObject mainMenu;
    [SerializeField] protected GameObject headphones;

    public void makeHeadphonesActive() {
        headphones.SetActive(true);
    }

    public void makeMainMenuActive() {
        headphones.SetActive(false);
        mainMenu.SetActive(true);
        PlayerPrefs.SetInt("hasSeenIntro", 1);
    }
}
