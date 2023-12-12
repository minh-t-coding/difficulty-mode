using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretManagerScript : MonoBehaviour {
    public static TurretManagerScript Instance;
    private bool shouldShoot = false;

    void Awake() {
        if (Instance == null) {
            Instance = this;
        }
    }

    public void TurretTurn() {
        if (shouldShoot) {
            foreach(Transform turret in transform) {
                TurretBehaviorScript turretBehaviorScript = turret.GetComponent<TurretBehaviorScript>();
                if (turretBehaviorScript != null && turretBehaviorScript.isActiveAndEnabled) {
                    turretBehaviorScript.TurretAttack();
                    // TODO: Turret attack indicator?
                }
            }
        }
        shouldShoot = !shouldShoot;
    }
}
