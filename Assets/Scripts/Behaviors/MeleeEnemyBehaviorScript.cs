using System.Collections;
using System.Collections.Generic;
using Toolbox;
using UnityEngine;
using UnityEngine.Tilemaps;

public class MeleeEnemyBehaviorScript : BaseEnemyBehavior {
    [SerializeField] protected float movementRange;
    [SerializeField] protected float attackRange;
    
    public override bool EnemyInRange() {
        Vector3 distanceFromPlayer = playerPosition.position - enemyDestination.position;
        return distanceFromPlayer.magnitude <= attackRange;
    }
    
    public override void EnemyMove() {
        // reset move flag
        isMoving = false;
        
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
                isMoving = true;
                movePath = newPosition - enemyDestination.position;
                enemyDestination.position = newPosition;
            }
        } else {
            base.EnemyMove();
        }

        if (isMoving) {
            // add movement command
            List<CommandManager.ICommand> commandList = new List<CommandManager.ICommand>
            {
                new MeleeEnemyMoveCommand(movePath, new Vector3(0, 0, 0), this)
            };
            CommandManager.Instance.AddCommand(this.GetInstanceID(), commandList);
        }
    }

    public override void EnemyAttacked(float damage)
    {
        base.EnemyAttacked(damage);

        // add melee enemy death command
        if (enemyHealth <= 0) {
            List<CommandManager.ICommand> commandList = new List<CommandManager.ICommand>
            {
                new MeleeEnemyDeathCommand(enemyDestination.position, Vector3.zero, this)
            };
            CommandManager.Instance.AddCommand(this.GetInstanceID(), commandList);
        }
    }

    public override void EnemyAttack()
    {
        if (EnemyInRange()) {
            HitEffect.CreateHitEffectStatic(playerPosition.position, new Color(1, 0, 0, 1));
            PlayerBehaviorScript.Instance.killPlayer();
            
            // add player attack command
            List<CommandManager.ICommand> commandList = new List<CommandManager.ICommand>
            {
                new MeleeEnemyAttackCommand(new Vector3(0, 0, 0), this)
            };
            CommandManager.Instance.AddCommand(this.GetInstanceID(), commandList);
        }   
    }
}