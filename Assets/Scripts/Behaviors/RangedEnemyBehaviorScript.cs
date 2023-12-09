using System;
using System.Collections;
using System.Collections.Generic;
using Toolbox;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Tilemaps;


public class RangedEnemyBehaviorScript : BaseEnemyBehavior {
    // Gun Variables
    [SerializeField] protected Transform shootingPoint;
    [SerializeField] protected GameObject projectilePrefab;
    [SerializeField] protected float movementRange;

    // Update is called once per frame
    protected override void Update() {        
        base.Update();
    }

    public override bool EnemyInRange() {
        Vector3 distanceFromPlayer = playerPosition.position - enemyDestination.position;
        
        // only shoot if in range and in line with player
        if ((distanceFromPlayer.magnitude <= movementRange) && (distanceFromPlayer.x == 0 || distanceFromPlayer.y == 0)) {   
            
            // don't shoot if a wall is in the way
            if (Physics2D.Linecast(enemyDestination.position, playerPosition.position, collisionMask))
            {
                return false;
            }

            return true;
        }

        return false;
    }

    public override void EnemyAttacked(float damage)
    {
        base.EnemyAttacked(damage);

        // add ranged enemy death command if dead
        if (enemyHealth <= 0) {
            List<CommandManager.ICommand> commandList = new List<CommandManager.ICommand>
            {
                new RangedEnemyDeathCommand(enemyDestination.position, Vector3.zero, this)
            };
            CommandManager.Instance.AddCommand(this.GetInstanceID(), commandList);
        }
    }

    public override void EnemyAttack() {
        Vector3 distanceFromPlayer = playerPosition.position - enemyDestination.position;
        
        Vector3 prefabDirection;
        int scriptDirection;

        // set prefab and script directions based on position relative to player
        if (distanceFromPlayer.x == 0) {
            if (distanceFromPlayer.y > 0) {
                prefabDirection = new Vector3(0, 0, 0);
                scriptDirection = (int) PlayerBehaviorScript.Direction.Up;
            } else {
                prefabDirection = new Vector3(0, 0, 180);
                scriptDirection = (int) PlayerBehaviorScript.Direction.Down;
            }
        } else {
            if (distanceFromPlayer.x < 0) {
                prefabDirection = new Vector3(0, 0, 90);
                scriptDirection = (int) PlayerBehaviorScript.Direction.Left;
            } else {
                prefabDirection = new Vector3(0, 0, -90);
                scriptDirection = (int) PlayerBehaviorScript.Direction.Right;
            }
        }

        GameObject projectile = Instantiate(projectilePrefab, shootingPoint.position, Quaternion.Euler(prefabDirection), ProjectileManagerScript.Instance.transform);
        ProjectileBehaviorScript projectileScript = projectile.GetComponent<ProjectileBehaviorScript>();
        projectileScript.setDirection(scriptDirection);
        projectileScript.SetPrefab(projectilePrefab);

        // add player attack command
        List<CommandManager.ICommand> commandList = new List<CommandManager.ICommand>
        {
            new RangedEnemyAttackCommand(distanceFromPlayer / distanceFromPlayer.magnitude, this)
        };
        CommandManager.Instance.AddCommand(this.GetInstanceID(), commandList);
    }

    public override void EnemyMove() {
        // reset move flag
        isMoving = false;
        
        // check if enemy is already in range
        Vector3 distanceFromPlayer = playerPosition.position - enemyDestination.position;
        if (distanceFromPlayer.magnitude < movementRange) { 
            // if enemy is already aligned with player, no need to move
            if (distanceFromPlayer.x == 0 || distanceFromPlayer.y == 0) {
                return;
            }

            // otherwise, move enemy so they are top/bottom/left/right of player
            Vector3 newPosition;
            if (Mathf.Abs(distanceFromPlayer.x) < Mathf.Abs(distanceFromPlayer.y)) {
                newPosition = enemyDestination.position + new Vector3(distanceFromPlayer.x / Mathf.Abs(distanceFromPlayer.x), 0);
            } else {
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
        
        // add movement command if enemy has moved
        if (isMoving) {
            List<CommandManager.ICommand> commandList = new List<CommandManager.ICommand>
            {
                new RangedEnemyMoveCommand(movePath, new Vector3(0, 0, 0), this)
            };
            CommandManager.Instance.AddCommand(this.GetInstanceID(), commandList);
        }
    }
}
