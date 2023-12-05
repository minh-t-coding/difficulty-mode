using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManagerScript : MonoBehaviour {
    [SerializeField] protected bool attackAndMoveSameTurn = false;
    
    public static EnemyManagerScript Instance;

    void Awake() {
        if (Instance == null) {
            Instance = this;
        }
    }

    public void EnemyTurn() {
        foreach(Transform enemy in transform) {
            if (enemy != null) {
                BaseEnemy enemyBehavior = enemy.GetComponent<BaseEnemy>();
                if (enemyBehavior != null) {
                    if (attackAndMoveSameTurn) {
                        enemyBehavior.EnemyMove();
                        if (enemyBehavior.EnemyInRange()) {
                            enemyBehavior.EnemyAttack();    
                        }
                    }
                    else {
                        if (enemyBehavior.EnemyInRange()) {
                            enemyBehavior.EnemyAttack();
                        } else {
                            enemyBehavior.EnemyMove();
                        }
                    }
                }
            }
        }
    }
}
