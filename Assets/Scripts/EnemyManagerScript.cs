using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManagerScript : MonoBehaviour {
    public void EnemyTurn() {
        foreach(Transform enemy in transform) {
            if (enemy != null) {
                EnemyScript enemyScript = enemy.GetComponent<EnemyScript>();
                if (enemyScript != null) {
                    enemyScript.EnemyMove();
                }
            }
        }
    }
}
