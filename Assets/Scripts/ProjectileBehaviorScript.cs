using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileBehaviorScript : MonoBehaviour {

    [SerializeField] protected float projectileSpeed;
    [SerializeField] protected float projectileDistance;
    [SerializeField] protected Transform projectileDestination;
    private int direction = 0;

    // Start is called before the first frame update
    void Start() {
        projectileDestination.parent = null;
        Destroy(this.gameObject, 10);
    }

    void Update() {
       transform.position = Vector3.MoveTowards(transform.position, projectileDestination.position, projectileSpeed * Time.deltaTime);
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
