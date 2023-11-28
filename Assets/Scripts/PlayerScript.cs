using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerScript : MonoBehaviour
{
    public static PlayerScript Instance;
    [SerializeField] protected float playerSpeed;

    void Awake() {
        if (Instance == null) {
            Instance = this;
        }
    }

    void Update()
    {
        if (Input.GetKey(KeyCode.W)) {
            transform.Translate(0, (playerSpeed * Time.deltaTime), 0);
        }

        if (Input.GetKey(KeyCode.A)) {
            transform.Translate((-playerSpeed * Time.deltaTime), 0, 0);
        }

        if (Input.GetKey(KeyCode.S)) {
            transform.Translate(0, (-playerSpeed * Time.deltaTime), 0);
        }

        if (Input.GetKey(KeyCode.D)) {
            transform.Translate((playerSpeed * Time.deltaTime), 0, 0);
        }
    }
}
