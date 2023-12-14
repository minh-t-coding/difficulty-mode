using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretManagerScript : MonoBehaviour {
    public static TurretManagerScript Instance;
    // private bool shouldShoot = true;

    void Awake() {
        if (Instance == null) {
            Instance = this;
        }
    }

    public void TurretTurn() {
        foreach(Transform turret in transform) {
            TurretBehaviorScript turretBehaviorScript = turret.GetComponent<TurretBehaviorScript>();
            if (turretBehaviorScript != null && turretBehaviorScript.isActiveAndEnabled) {
                turretBehaviorScript.TurretAttack();
                // TODO: Turret attack indicator?
            }
        }
    }

    public HashSet<Vector3> getTurretPositions() {
        HashSet<Vector3> positions = new HashSet<Vector3>();
        foreach(Transform turret in transform) {
            if (turret!=null) {
            positions.Add(turret.position);
           }
        }
        return positions;
    }
}
