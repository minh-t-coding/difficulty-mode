using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileBehaviorScript : MonoBehaviour {
    [SerializeField] protected float projectileSpeed;
    [SerializeField] protected float projectileDistance;
    [SerializeField] protected Transform projectileDestination;
    [SerializeField] protected LayerMask collisionMask;
    protected Transform playerPosition;
    private int direction = 0;
    private bool isProjectileMoving;
    private GameObject projectileMovePoint;
    
    // Start is called before the first frame update
    void Start() {
        this.projectileMovePoint = projectileDestination.gameObject;
        playerPosition = PlayerScript.Instance.getMovePoint();
        projectileDestination.parent = null;

        // will destroy the projectile by default after 20 seconds
        // Destroy(this.gameObject, 20);
        // Destroy(this.projectileMovePoint, 20);
    }

    void Update() {
        isProjectileMoving = true;
        transform.position = Vector3.MoveTowards(transform.position, projectileDestination.position, projectileSpeed * Time.deltaTime);

        if (Vector3.Distance(transform.position, projectileDestination.position) <= Mathf.Epsilon) {
            isProjectileMoving = false;
        }

        // Check if projectile position will overlap with collisionMask
        if (Physics2D.OverlapCircle(transform.position, .1f, collisionMask)) {
            DestroyProjectile();
        }

        // Check if projectile position touches Player
        if (Vector3.Distance(transform.position, playerPosition.position) < .05f) {
            DestroyProjectile();
            PlayerScript.Instance.killPlayer();
        }
    }

    private void DestroyProjectile() {
        Destroy(this.gameObject);
        Destroy(this.projectileMovePoint);
    }

    public void ProjectileMove() {
        switch (this.direction) {
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

    public bool getIsProjectileMoving() {
        return isProjectileMoving;
    }

    public void setDirection(int newDirection) {
        this.direction = newDirection;
    }
}
