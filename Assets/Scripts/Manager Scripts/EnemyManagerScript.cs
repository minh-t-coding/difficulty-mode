using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManagerScript : MonoBehaviour {
    [SerializeField] protected bool attackAndMoveSameTurn = false;
    
    public static EnemyManagerScript Instance;

    private HashSet<GameObject> enemyMovepoints = new HashSet<GameObject>();
    private int meleeEnemyCount;
    private int rangedEnemyCount;

    void Awake() {
        if (Instance == null) {
            Instance = this;
        }

        // Save References of all enemy movepoints
        foreach(Transform enemy in transform) {
            if (enemy.CompareTag("MeleeEnemy")) {
                meleeEnemyCount++;
            } else if (enemy.CompareTag("RangedEnemy")) {
                rangedEnemyCount++;
            }
            GameObject movePoint = enemy.GetChild(0).gameObject; // Getting first child, maybe better way to do this?
            enemyMovepoints.Add(movePoint);
        }
    }

    public void subtractMeleeEnemyCount() {
        meleeEnemyCount--;
    }

    public void subtractRangedEnemyCount() {
        rangedEnemyCount--;
    }

    public Tuple<int, int> getEnemyCounts() {
        return new Tuple <int, int>(meleeEnemyCount, rangedEnemyCount);
    }

    public HashSet<GameObject> getEnemyMovepoints() {
        return this.enemyMovepoints;
    }

    public void EnemyAttacked(Vector3 enemyPosition, float damage) {
        foreach(Transform enemy in transform) {
            if (enemy != null) {
                BaseEnemyBehavior enemyBehavior = enemy.GetComponent<BaseEnemyBehavior>();

                if (enemyBehavior.transform.position == enemyPosition) {
                    enemyBehavior.EnemyAttacked(damage);
                    return;
                }
            }
        }
    }

    public void EnemyTurn() {
        List<Transform> enemies = new List<Transform>();

         // Collect all enemy transforms in a list
        foreach (Transform enemy in transform) {
            if (enemy != null) {
                enemies.Add(enemy);
            }
        }

        // Sort the list based on distance to the player
        enemies.Sort((a, b) =>
            Vector3.Distance(a.position, PlayerBehaviorScript.Instance.getMovePoint().position)
                .CompareTo(Vector3.Distance(b.position, PlayerBehaviorScript.Instance.getMovePoint().position)));

        foreach(Transform enemy in enemies) {
            BaseEnemyBehavior enemyBehavior = enemy.GetComponent<BaseEnemyBehavior>();
            if (enemyBehavior != null && enemyBehavior.isActiveAndEnabled) {
                if (attackAndMoveSameTurn) {
                    enemyBehavior.EnemyMove();
                    if (enemyBehavior.EnemyInRange()) {
                        enemyBehavior.EnemyAttack();    
                    }
                }
                else {
                    if (enemyBehavior.EnemyInRange()) {
                        enemyBehavior.EnemyAttack();
                    } 
                    else {
                        enemyBehavior.EnemyMove();
                    }
                }
            }
        }
    }
}
