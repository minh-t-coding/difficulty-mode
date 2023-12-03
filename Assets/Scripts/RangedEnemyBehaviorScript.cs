using System;
using System.Collections;
using System.Collections.Generic;
using Toolbox;
using UnityEngine;
using UnityEngine.Tilemaps;


public class RangedEnemyBehaviorScript : MonoBehaviour, IEnemyBehavior {
    // Gun Variables
    [SerializeField] protected Transform shootingPoint;
    [SerializeField] protected Transform projectileManager;
    [SerializeField] protected GameObject projectilePrefab;
    [SerializeField] protected float enemySpeed;
    [SerializeField] protected float movementRange;
    [SerializeField] protected Transform enemyDestination;
    private Transform playerPosition;
    private Tilemap tileMap;
    private List<Vector3> nextMoves;

    // Start is called before the first frame update
    void Start()
    {
        enemyDestination.parent = null;
        playerPosition = PlayerScript.Instance.getMovePoint();
        tileMap = GameObject.Find("Top").GetComponent<Tilemap>();
        nextMoves = new List<Vector3>();
    }

    // Update is called once per frame
    void Update() {
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

        transform.position = Vector3.MoveTowards(transform.position, enemyDestination.position, enemySpeed * Time.deltaTime);
    }

    public void EnemyMove() {
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
        
        // Find shortest path to player's new position
        nextMoves = AStar.FindPathClosest(tileMap, enemyDestination.position, playerPosition.position);

        if(nextMoves.Count > 1) {
            // FindPathClosest returns current position as first element of the list
            Vector3 nextMove = nextMoves[1];
            Vector3 pathToPlayer = nextMove - enemyDestination.position;

            // Enemy only moves one unit in eight cardinal directions
            if (pathToPlayer.x != 0) {
                enemyDestination.position += pathToPlayer / Mathf.Abs(pathToPlayer.x);
            } else if (pathToPlayer.y != 0) {
                enemyDestination.position += pathToPlayer / Mathf.Abs(pathToPlayer.y);
            }
        }
    }
}
