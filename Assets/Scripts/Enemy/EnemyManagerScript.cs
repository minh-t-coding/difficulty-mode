using System;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManagerScript : MonoBehaviour {
    [SerializeField] protected bool attackAndMoveSameTurn = false;
    
    public static EnemyManagerScript Instance;

    private HashSet<GameObject> enemyMovepoints = new HashSet<GameObject>();
    protected int meleeEnemyCount;
    protected int rangedEnemyCount;

    protected List<BaseEnemy> currEnemies;

    

    void Awake() {
        if (Instance == null) {
            Instance = this;
        }
        currEnemies = new List<BaseEnemy>();
    }

    

    public void addEnemy(BaseEnemy e) {
        currEnemies.Add(e);
        if (e.CompareTag("MeleeEnemy")) {
            meleeEnemyCount++;
        } else if (e.CompareTag("RangedEnemy")) {
            rangedEnemyCount++;
        }
    }

    public void removeEnemy(BaseEnemy e) {
        currEnemies.Remove(e);
        if (e.CompareTag("MeleeEnemy")) {
            meleeEnemyCount--;
        } else if (e.CompareTag("RangedEnemy")) {
            rangedEnemyCount--;
        }
    }

    

    public HashSet<GameObject> getEnemyMovepoints() {
        HashSet<GameObject> e = new HashSet<GameObject>();
        foreach(BaseEnemy enemy in currEnemies) {
            if (enemy!=null) {
            e.Add(enemy.getMovePoint());
           }
        }
        return e;
    }

    public bool EnemyAttacked(Vector3 enemyPosition, float damage) {
        foreach(Transform enemy in transform) {
            if (enemy != null) {
                BaseEnemy enemyBehavior = enemy.GetComponent<BaseEnemy>();

                if (enemyBehavior.transform.position == enemyPosition) {
                    enemyBehavior.EnemyAttacked(damage);
                    return true;
                }
            }
        }
        return false;
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

    public Tuple<int, int> getEnemyCounts() {
        return new Tuple <int, int>(meleeEnemyCount, rangedEnemyCount);
    }
}
