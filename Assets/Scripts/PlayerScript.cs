using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerScript : MonoBehaviour
{
    public static PlayerScript Instance;
    [SerializeField] protected float playerSpeed;
    [SerializeField] protected Transform destination;
    [SerializeField] protected LayerMask collisionMask;

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

        if (Vector3.Distance(transform.position, destination.position) <= .05f) // Check if Player is near destination
        {

            if (Mathf.Abs(Input.GetAxisRaw("Horizontal")) == 1f)
            {
                if (!Physics2D.OverlapCircle(destination.position + new Vector3(Input.GetAxisRaw("Horizontal"), 0f, 0f), .1f, collisionMask)) // Check if new destination position will overlap with collisionMask
                {
                    destination.position += new Vector3(Input.GetAxisRaw("Horizontal"), 0f, 0f);
                }
            }

            if (Mathf.Abs(Input.GetAxisRaw("Vertical")) == 1f)
            {
                if (!Physics2D.OverlapCircle(destination.position + new Vector3(0f, Input.GetAxisRaw("Vertical"), 0f), .1f, collisionMask))
                {
                    destination.position += new Vector3(0f, Input.GetAxisRaw("Vertical"), 0f);
                }
              
            }
        }
    }
}
