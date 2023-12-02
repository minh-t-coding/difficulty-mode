using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileBehaviorScript : MonoBehaviour {

    [SerializeField] protected float projectileSpeed;
    private Rigidbody2D rb;

    // Start is called before the first frame update
    void Start() {
        rb = GetComponent<Rigidbody2D>();
        rb.velocity = transform.right * projectileSpeed;
        Destroy(this.gameObject, 2f);
    }

    void FixedUpdate() {
        rb.velocity = transform.up * projectileSpeed;
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        Destroy(this.gameObject, 2f);
    }
}
