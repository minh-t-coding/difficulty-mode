using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerScript : MonoBehaviour
{
    public static PlayerScript Instance;
    [SerializeField] protected float playerSpeed;
    [SerializeField] protected Transform destination;

    void Awake() {
        if (Instance == null) {
            Instance = this;
        }
    }

    void Start()
    {
        destination.parent = null;
    }

    void Update()
    {
        // Move Player to destination point
        transform.position = Vector3.MoveTowards(transform.position, destination.position, playerSpeed * Time.deltaTime);

        if (Vector3.Distance(transform.position, destination.position) <= .05f)
        {

            if (Mathf.Abs(Input.GetAxisRaw("Horizontal")) == 1f)
            {
                destination.position += new Vector3(Input.GetAxisRaw("Horizontal"), 0f, 0f);
            }

            if (Mathf.Abs(Input.GetAxisRaw("Vertical")) == 1f)
            {
                destination.position += new Vector3(0f, Input.GetAxisRaw("Vertical"), 0f);
            }
        }
    }
}
