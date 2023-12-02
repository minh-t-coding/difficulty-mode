using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileBehaviorScript : MonoBehaviour {
    [SerializeField] protected float projectileSpeed;
    [SerializeField] protected float projectileDistance;
    [SerializeField] protected Transform projectileDestination;
    [SerializeField] protected LayerMask collisionMask;
    private int direction = 0;
    private GameObject projectileMovePoint;

    // Start is called before the first frame update
    void Start() {
        this.projectileMovePoint = projectileDestination.gameObject;
        projectileDestination.parent = null;

        // will destroy the projectile by default after 20 seconds
        Destroy(this.gameObject, 20);
        Destroy(this.projectileMovePoint, 20);
    }

    void Update() {
        transform.position = Vector3.MoveTowards(transform.position, projectileDestination.position, projectileSpeed * Time.deltaTime);
        
        if (Physics2D.OverlapCircle(transform.position, .1f, collisionMask)) { // Check if projectile position will overlap with collisionMask
            Destroy(this.gameObject);
            Destroy(this.projectileMovePoint);
        }
    }

    public void ProjectileMove() {
        switch (direction) {
            case 0:
                projectileDestination.position += new Vector3(0f, projectileDistance, 0f);
                break;
            case 1:
                projectileDestination.position += new Vector3(0f, -projectileDistance, 0f);
                break;
            case 2:
                projectileDestination.position += new Vector3(-projectileDistance, 0f, 0f);
                break;
            case 3:
                projectileDestination.position += new Vector3(projectileDistance, 0f, 0f);
                break;
        }
    }

    public void setDirection(int newDirection) {
        this.direction = newDirection;
    }
}
