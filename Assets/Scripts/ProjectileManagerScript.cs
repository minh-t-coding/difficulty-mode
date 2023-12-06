using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileManagerScript : MonoBehaviour
{
    public static ProjectileManagerScript Instance;

    private bool areProjectilesInAction;

    void Awake() {
        if (Instance == null) {
            Instance = this;
        }
    }

    void Update() {
        foreach(Transform projectile in transform) {
            if (projectile != null) {
                ProjectileBehaviorScript projectileBehaviorScript = projectile.GetComponent<ProjectileBehaviorScript>();
                if (projectileBehaviorScript != null) {
                    this.areProjectilesInAction = projectileBehaviorScript.getIsProjectileMoving();
                }
            }
        }
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
