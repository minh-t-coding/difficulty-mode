using System;
using System.Collections.Generic;

// using System.Numerics;
using Toolbox;
using UnityEngine;
using UnityEngine.Tilemaps;

public class BaseEnemyBehavior : MonoBehaviour
{
    [SerializeField] protected float enemyHealth;
    [SerializeField] protected float enemySpeed;
    [SerializeField] protected Transform enemyDestination;
    [SerializeField] protected LayerMask collisionMask;

    protected GameObject movePoint;
    protected Tilemap tileMap;
    protected Transform playerPosition;
    protected List<Vector3> nextMoves;
    protected Vector3 movePath;
    protected bool isMoving;
    
    protected virtual void Start() {
        movePoint = enemyDestination.gameObject;
        enemyDestination.parent = null;
        playerPosition = PlayerBehaviorScript.Instance.getMovePoint();
        tileMap = GameObject.Find("Top").GetComponent<Tilemap>();
        nextMoves = new List<Vector3>();
    }

    protected virtual void Update() {
        transform.position = Vector3.MoveTowards(transform.position, enemyDestination.position, enemySpeed * Time.deltaTime);
    }

    public virtual void MoveDestination(Vector3 direction) {
        this.enemyDestination.position += direction;
    }

    public virtual void Spawn(Vector3 direction, Vector3 orientation) {
        // TODO: add logic here to respawn
    }

    public virtual bool EnemyInRange() {
        return false;
    }

    public virtual void EnemyAttacked(float damage) {
        enemyHealth -= damage;

        if (enemyHealth <= 0) {
            Debug.Log("enemy killed!");
            this.gameObject.SetActive(false);
            movePoint.SetActive(false);
        }
    }
    
    public virtual void EnemyAttack() {}

    public virtual void EnemyUndoAttack(Vector3 attackDir) {}

    public virtual void EnemyMove() {
        // reset isMoving flag
        isMoving = false;

        // Find shortest path to player's new position
        nextMoves = AStar.FindPathClosest(tileMap, enemyDestination.position, playerPosition.position);
        HashSet<GameObject> enemyMovepoints = EnemyManagerScript.Instance.getEnemyMovepoints();
        HashSet<Vector3> movepointPositions = new HashSet<Vector3>();
        foreach (GameObject movepoint in enemyMovepoints) {
            if (movepoint.activeSelf) {
                movepointPositions.Add(movepoint.transform.position);
            }
        }

        if(nextMoves.Count > 1) {
            // FindPathClosest returns current position as first element of the list
            Vector3 nextMove = nextMoves[1];
           movePath = nextMove - enemyDestination.position;

            // Enemy only moves one unit in eight cardinal directions
            if (movePath.x != 0) {
                movePath = movePath / Mathf.Abs(movePath.x);
            } else if (movePath.y != 0) {
                movePath = movePath / Mathf.Abs(movePath.y);
            }
            
            // Only move if next move is not on another enemy
            if (!movepointPositions.Contains(enemyDestination.position + movePath)) {
                
                isMoving = true;
                enemyDestination.position += movePath;
            }
        }
    }
}