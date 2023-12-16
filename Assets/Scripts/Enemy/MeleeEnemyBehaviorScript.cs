using System.Collections;
using System.Collections.Generic;
using Toolbox;
using UnityEngine;
using UnityEngine.Tilemaps;

public class MeleeEnemyBehaviorScript : BaseEnemy {

    [SerializeField] protected float movementRange;

    [SerializeField] protected float attackRange;

    public override bool EnemyInRange() {
        Vector3 distanceFromPlayer = playerPosition.position - enemyDestination.position;
        return distanceFromPlayer.magnitude <= attackRange;
    }

    public override void EnemyAttack() {
        if (EnemyInRange() && !PlayerInputManager.Instance.getIsStickoMode()) {
            ChangeEnemyAnimationState(enemyType + ENEMY_ATTACK, playerPosition.position - enemyDestination.position);
            base.EnemyAttack();
            HitEffect.CreateHitEffectStatic(playerPosition.position, new Color(1, 0, 0, 1));
            PlayerBehaviorScript.Instance.killPlayer();
        }
    }

    public override void EnemyMove() {
        HashSet<GameObject> enemyMovepoints = EnemyManagerScript.Instance.getEnemyMovepoints();
        HashSet<Vector3> movepointPositions = new HashSet<Vector3>();
        HashSet<Vector3> turretPositions = TurretManagerScript.Instance.getTurretPositions();
        foreach (GameObject movepoint in enemyMovepoints) {
            if (movepoint.activeSelf) {
                movepointPositions.Add(movepoint.transform.position);
            }
        }
        
        // check if enemy is already in range
        Vector3 distanceFromPlayer = playerPosition.position - enemyDestination.position;
        if (distanceFromPlayer.magnitude < movementRange) {
            //move enemy so they are top/bottom/left/right of player
            Vector3 newPosition = enemyDestination.position;
            if (Mathf.Abs(distanceFromPlayer.x) < Mathf.Abs(distanceFromPlayer.y)) {
                if (distanceFromPlayer.x!=0) {
                    newPosition = enemyDestination.position + new Vector3(distanceFromPlayer.x / Mathf.Abs(distanceFromPlayer.x), 0);
                } 
            }
            else {
                if (distanceFromPlayer.y!=0) {
                    newPosition = enemyDestination.position + new Vector3(0, distanceFromPlayer.y / Mathf.Abs(distanceFromPlayer.y));
                } 
            }

            // only alter path if there is no collision 
            
            if (!turretPositions.Contains(newPosition) && 
                !movepointPositions.Contains(newPosition) && 
                tileMap.IsCellEmpty(newPosition)) {
                Vector3 pathToPlayer = newPosition - enemyDestination.position;
                Debug.Log(pathToPlayer);
                enemyDestination.position = newPosition;
                ChangeEnemyAnimationState(enemyType + ENEMY_MOVE, pathToPlayer);
                return;
            }
        }

        base.EnemyMove();
    }
}