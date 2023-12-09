using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManagerScript : MonoBehaviour {
    [SerializeField] protected bool attackAndMoveSameTurn = false;
    
    public static EnemyManagerScript Instance;

    private HashSet<GameObject> enemyMovepoints = new HashSet<GameObject>();

    void Awake() {
        if (Instance == null) {
            Instance = this;
        }

        // Save References of all enemy movepoints
        foreach(Transform enemy in transform) {
            GameObject movePoint = enemy.GetChild(0).gameObject; // Getting first child, maybe better way to do this?
            enemyMovepoints.Add(movePoint);
        }
    }

    public HashSet<GameObject> getEnemyMovepoints() {
        return this.enemyMovepoints;
    }

    public void EnemyAttacked(Vector3 enemyPosition, float damage) {
        foreach(Transform enemy in transform) {
            if (enemy != null) {
                BaseEnemy enemyBehavior = enemy.GetComponent<BaseEnemy>();

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
            Vector3.Distance(a.position, PlayerScript.Instance.getMovePoint().position)
                .CompareTo(Vector3.Distance(b.position, PlayerScript.Instance.getMovePoint().position)));

        foreach(Transform enemy in enemies) {
            BaseEnemy enemyBehavior = enemy.GetComponent<BaseEnemy>();
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
                enemyBehavior.UpdateIndicator();
            }
        }
    }
}
