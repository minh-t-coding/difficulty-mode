using System;
using System.Collections;
using System.Collections.Generic;
using Toolbox;
using UnityEngine;
using UnityEngine.Tilemaps;


public class RangedEnemyBehaviorScript : BaseEnemy {
    // Gun Variables
    [SerializeField] protected Transform shootingPoint;
    [SerializeField] protected Transform projectileManager;
    [SerializeField] protected GameObject projectilePrefab;
    [SerializeField] protected float movementRange;

    // Update is called once per frame
    protected override void Update() {
        GameObject projectile;
        if (Input.GetKeyDown(KeyCode.I)) { // Up
            projectile = Instantiate(projectilePrefab, shootingPoint.position, Quaternion.Euler(new Vector3(0, 0, 0)), projectileManager);
            projectile.GetComponent<ProjectileBehaviorScript>().setDirection(0);
        }
        if (Input.GetKeyDown(KeyCode.K)) { // Down
            projectile = Instantiate(projectilePrefab, shootingPoint.position, Quaternion.Euler(new Vector3(0, 0, 180)), projectileManager);
            projectile.GetComponent<ProjectileBehaviorScript>().setDirection(1);
        }
        if (Input.GetKeyDown(KeyCode.J)) { // Left
            projectile = Instantiate(projectilePrefab, shootingPoint.position, Quaternion.Euler(new Vector3(0, 0, 90)), projectileManager);
            projectile.GetComponent<ProjectileBehaviorScript>().setDirection(2);
        }
        if (Input.GetKeyDown(KeyCode.L)) { // Right
            projectile = Instantiate(projectilePrefab, shootingPoint.position, Quaternion.Euler(new Vector3(0, 0, -90)), projectileManager);
            projectile.GetComponent<ProjectileBehaviorScript>().setDirection(3);
        }
        
        base.Update();
    }

    public override void EnemyMove() {
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
                enemyDestination.position = newPosition;
            }

            return;
        }
        
        base.EnemyMove();
    }
}
