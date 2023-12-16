using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;
public class GlobalVolume : MonoBehaviour
{
    public static GlobalVolume Instance;

    [SerializeField] private GameObject vignette;

    [SerializeField] private GameObject bloom;

    void Awake() {
        if (Instance==null) {
            Instance = this;
            toggleVignette(true);
        }
    }

    public void toggleBloom(bool b) {
        bloom.SetActive(b);
    }



    public void toggleVignette(bool b) {
        vignette.SetActive(b);
        bloom.SetActive(!b);
    } 
}
