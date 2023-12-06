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
        int projectileCount = transform.childCount; 
        int notMovingProjectileCount = 0; 

        foreach(Transform projectile in transform) {
            if (projectile != null) {
                ProjectileBehaviorScript projectileBehaviorScript = projectile.GetComponent<ProjectileBehaviorScript>();
                if (projectileBehaviorScript != null) {
                    if (!projectileBehaviorScript.getIsProjectileMoving()) {
                        notMovingProjectileCount++;
                    }
                    this.areProjectilesInAction =  this.areProjectilesInAction || projectileBehaviorScript.getIsProjectileMoving();

                    if (notMovingProjectileCount.Equals(projectileCount)) {
                        this.areProjectilesInAction = false;
                    }
                }
            }
        }
        notMovingProjectileCount = 0;
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
