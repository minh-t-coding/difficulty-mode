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
        // check if enemy is already in range
        Vector3 distanceFromPlayer = playerPosition.position - enemyDestination.position;
        if (distanceFromPlayer.magnitude < movementRange) {
            //move enemy so they are top/bottom/left/right of player
            Vector3 newPosition = enemyDestination.position;
            if (Mathf.Abs(distanceFromPlayer.x) < Mathf.Abs(distanceFromPlayer.y)) {
                if (distanceFromPlayer.x!=0) {
                    newPosition = enemyDestination.position + new Vector3(distanceFromPlayer.x / Mathf.Abs(distanceFromPlayer.x), 0);
                } else {
                    return;
                }
            }
            else {
                if (distanceFromPlayer.y!=0) {
                    newPosition = enemyDestination.position + new Vector3(0, distanceFromPlayer.y / Mathf.Abs(distanceFromPlayer.y));
                } else {
                    return;
                }
            }

            // only alter path if there is no collision 
            
            //if (tileMap.IsCellEmpty(newPosition)) {
                
            //}
            Vector3 pathToPlayer = newPosition - enemyDestination.position;
            enemyDestination.position = newPosition;
            ChangeEnemyAnimationState(enemyType + ENEMY_MOVE, pathToPlayer);
            return;
        }

        base.EnemyMove();
    }
}