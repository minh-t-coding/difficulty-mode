using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManagerScript : MonoBehaviour {
    [SerializeField] protected bool attackAndMoveSameTurn = false;
    
    public static EnemyManagerScript Instance;

    private HashSet<GameObject> enemyMovepoints = new HashSet<GameObject>();

    protected List<BaseEnemy> currEnemies;

    

    void Awake() {
        if (Instance == null) {
            Instance = this;
        }
        currEnemies = new List<BaseEnemy>();
        // Save References of all enemy movepoints
    }

    

    public void addEnemy(BaseEnemy e) {
        currEnemies.Add(e);
    }

    public void removeEnemy(BaseEnemy e) {
        currEnemies.Remove(e);
    }

    

    public HashSet<GameObject> getEnemyMovepoints() {
        HashSet<GameObject> e = new HashSet<GameObject>();
        foreach(BaseEnemy enemy in currEnemies) {
            if (enemy!=null) {
            e.Add(enemy.getMovePoint());
           } // Getting first child, maybe better way to do this?
        }
        return e;
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
