using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeatmapIcon : MonoBehaviour
{
    [SerializeField] private KeyCode key;
    [SerializeField] private GameObject on;

    [SerializeField] private GameObject off;

    void Start()
    {
        toggle(false);
    }

    public KeyCode getKeyCode() {
        return key;
    }

    private void toggle(bool b) {
        off.SetActive(!b);
        on.SetActive(b);
    }

    // Update is called once per frame
    void Update()
    {
        toggle(Input.GetKey(key));
    }
}
