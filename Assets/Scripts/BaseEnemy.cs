using System;
using System.Collections.Generic;
using Toolbox;
using UnityEngine;
using UnityEngine.Tilemaps;

public class BaseEnemy : MonoBehaviour
{
    [SerializeField] protected float enemyHealth;
    [SerializeField] protected float enemySpeed;
    [SerializeField] protected Transform enemyDestination;
    [SerializeField] protected LayerMask collisionMask;

    protected GameObject movePoint;
    protected Tilemap tileMap;
    protected Transform playerPosition;
    protected List<Vector3> nextMoves;

    protected ActionIndicator myActionIndicator;
    
    protected virtual void Start() {
        movePoint = enemyDestination.gameObject;
        enemyDestination.parent = null;
        playerPosition = PlayerScript.Instance.getMovePoint();
        tileMap = GameObject.Find("Top").GetComponent<Tilemap>();
        myActionIndicator = ActionIndicator.Create(transform);
        nextMoves = new List<Vector3>();
    }

    protected virtual void Update() {
        transform.position = Vector3.MoveTowards(transform.position, enemyDestination.position, enemySpeed * Time.deltaTime);
    }

    public virtual bool EnemyInRange() {
        return false;
    }

    public virtual void UpdateIndicator() {
        if (EnemyInRange()) {
            myActionIndicator.SetAction(ActionIndicator.ActionIndicatorActions.Attack, GetAttackDirection());
        } else {
            myActionIndicator.SetAction(ActionIndicator.ActionIndicatorActions.Move, GetAttackDirection());
        }
    }
    public virtual Vector3 GetAttackDirection() {
        Vector3 distanceFromPlayer = playerPosition.position - enemyDestination.position;
        return distanceFromPlayer.normalized;
    }


    public virtual void EnemyAttacked(float damage) {
        enemyHealth -= damage;

        if (enemyHealth <= 0) {
            Debug.Log("enemy killed!");
            this.gameObject.SetActive(false);
            movePoint.SetActive(false);
            myActionIndicator.gameObject.SetActive(false);
        }
    }
    
    public virtual void EnemyAttack() {}

    public virtual void EnemyMove() {
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
            Vector3 pathToPlayer = nextMove - enemyDestination.position;

            // Enemy only moves one unit in eight cardinal directions
            if (pathToPlayer.x != 0) {
                pathToPlayer = pathToPlayer / Mathf.Abs(pathToPlayer.x);
            } else if (pathToPlayer.y != 0) {
                pathToPlayer = pathToPlayer / Mathf.Abs(pathToPlayer.y);
            }
            
            // Only move if next move is not on another enemy
            if (!movepointPositions.Contains(enemyDestination.position + pathToPlayer)) {
                enemyDestination.position += pathToPlayer;
            }
        }
    }
}