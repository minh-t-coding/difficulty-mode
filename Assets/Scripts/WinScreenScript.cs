using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class WinScreenScript : MonoBehaviour
{
    public void GoToMainMenu() {
        SceneManager.LoadScene("MenuV2");
    }
}
