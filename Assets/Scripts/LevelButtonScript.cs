using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelButtonScript : MonoBehaviour {

    [SerializeField] int level;
    public void OnButtonClick() {
        Debug.Log("he");
        SceneManager.LoadScene(level+1);
    }
    
}
