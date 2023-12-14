using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class AGameByScript : MonoBehaviour
{
    [SerializeField] protected GameObject background;

    public void makeInactive() {
        gameObject.SetActive(false);
    }

    public void makeBackgroundActive() {
        background.SetActive(true);
    }
}
