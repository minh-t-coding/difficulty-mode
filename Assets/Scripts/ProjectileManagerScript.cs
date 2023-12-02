using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileManagerScript : MonoBehaviour
{
    public static ProjectileManagerScript Instance;

    void Awake() {
        if (Instance == null) {
            Instance = this;
        }
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
