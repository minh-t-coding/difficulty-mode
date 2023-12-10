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
        if (EnemyInRange()) {
            HitEffect.CreateHitEffectStatic(playerPosition.position, new Color(1, 0, 0, 1));
            PlayerScript.Instance.killPlayer();
        }
    }

    public override void EnemyMove() {
        // check if enemy is already in range
        Vector3 distanceFromPlayer = playerPosition.position - enemyDestination.position;
        if (distanceFromPlayer.magnitude < movementRange) {
            //move enemy so they are top/bottom/left/right of player
            Vector3 newPosition;
            if (Mathf.Abs(distanceFromPlayer.x) < Mathf.Abs(distanceFromPlayer.y)) {
                newPosition = enemyDestination.position + new Vector3(distanceFromPlayer.x / Mathf.Abs(distanceFromPlayer.x), 0);
            }
            else {
                newPosition = enemyDestination.position + new Vector3(0, distanceFromPlayer.y / Mathf.Abs(distanceFromPlayer.y));
            }

            // only alter path if there is no collision 
            if (tileMap.IsCellEmpty(newPosition)) {
                enemyDestination.position = newPosition;
            }

            return;
        }

        base.EnemyMove();
    }
}