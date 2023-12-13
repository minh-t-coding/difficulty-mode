using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileManagerScript : MonoBehaviour
{
    public static ProjectileManagerScript Instance;

    private bool areProjectilesInAction = false;

    

    void Awake() {
        if (Instance == null) {
            Instance = this;
        }
    }

    void Update() {
        int notMovingProjectileCount = 0; 

        if (transform.childCount.Equals(0)) {
            this.areProjectilesInAction = false;
        }

        foreach(Transform projectile in transform) {
            if (projectile != null) {
                ProjectileBehaviorScript projectileBehaviorScript = projectile.GetComponent<ProjectileBehaviorScript>();
                if (projectileBehaviorScript != null) {
                    if (!projectileBehaviorScript.getIsProjectileMoving()) {
                        notMovingProjectileCount++;
                    }
                    this.areProjectilesInAction =  this.areProjectilesInAction || projectileBehaviorScript.getIsProjectileMoving();

                    // if all projectiles are not moving, reset the flag
                    if (notMovingProjectileCount.Equals(transform.childCount)) {
                        this.areProjectilesInAction = false;
                    }
                }
            } 
        }
    }

    public bool ProjectileAttacked(Vector3 projectilePosition, Vector3 playerPos) {
        Vector3 newDir = projectilePosition - playerPos;

        foreach (Transform projectile in transform) {
            if (projectile != null) {
                ProjectileBehaviorScript projectileBehavior = projectile.GetComponent<ProjectileBehaviorScript>();

                if (projectileBehavior.transform.position == projectilePosition) {
                    // add reflect logic here
                    // Debug.Log("projectile hit");
                    Debug.Log(projectileBehavior);
                    projectileBehavior.projectileReflected(newDir.normalized);
                    return true;
                }
            }
        }

        return false;
    }

    public bool getAreProjectilesInAction() {
        return this.areProjectilesInAction;
    }

    public void ProjectileTurn() {
        foreach(Transform projectile in transform) {
            if (projectile != null) {
                ProjectileBehaviorScript projectileBehaviorScript = projectile.GetComponent<ProjectileBehaviorScript>();
                if (projectileBehaviorScript != null) {
                    projectileBehaviorScript.ProjectileMove();
                }
            }
        }
    }
}
