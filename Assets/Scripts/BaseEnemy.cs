using System.Collections.Generic;
using Toolbox;
using UnityEngine;
using UnityEngine.Tilemaps;

public class BaseEnemy : MonoBehaviour
{
    [SerializeField] protected float enemySpeed;
    [SerializeField] protected Transform enemyDestination;

    protected Tilemap tileMap;
    protected Transform playerPosition;
    protected List<Vector3> nextMoves;
    
    protected virtual void Start() {
        enemyDestination.parent = null;
        playerPosition = PlayerScript.Instance.getMovePoint();
        tileMap = GameObject.Find("Top").GetComponent<Tilemap>();
        nextMoves = new List<Vector3>();
    }

    protected virtual void Update() {
        transform.position = Vector3.MoveTowards(transform.position, enemyDestination.position, enemySpeed * Time.deltaTime);
    }

    public virtual void EnemyMove() {
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