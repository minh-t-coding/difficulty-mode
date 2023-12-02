using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class RangedEnemyBehaviorScript : MonoBehaviour {
    // Gun Variables
    [SerializeField] protected Transform shootingPoint;
    [SerializeField] protected Transform projectileManager;
    [SerializeField] protected GameObject projectilePrefab;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update() {
        GameObject projectile;
        if (Input.GetKeyDown(KeyCode.I)) { // Up
            projectile = Instantiate(projectilePrefab, shootingPoint.position, Quaternion.Euler(new Vector3(0, 0, 0)), projectileManager);
            projectile.GetComponent<ProjectileBehaviorScript>().setDirection(0);
        }
        if (Input.GetKeyDown(KeyCode.K)) { // Down
            projectile = Instantiate(projectilePrefab, shootingPoint.position, Quaternion.Euler(new Vector3(0, 0, 180)), projectileManager);
            projectile.GetComponent<ProjectileBehaviorScript>().setDirection(1);
        }
        if (Input.GetKeyDown(KeyCode.J)) { // Left
            projectile = Instantiate(projectilePrefab, shootingPoint.position, Quaternion.Euler(new Vector3(0, 0, 90)), projectileManager);
            projectile.GetComponent<ProjectileBehaviorScript>().setDirection(2);
        }
        if (Input.GetKeyDown(KeyCode.L)) { // Right
            projectile = Instantiate(projectilePrefab, shootingPoint.position, Quaternion.Euler(new Vector3(0, 0, -90)), projectileManager);
            projectile.GetComponent<ProjectileBehaviorScript>().setDirection(3);
        }
    }
}
