using System.Collections;
using System.Collections.Generic;
using Toolbox;
using UnityEngine;
using UnityEngine.Tilemaps;

public class EnemyScript : MonoBehaviour {
    [SerializeField] protected float enemySpeed;
    [SerializeField] protected Transform enemyDestination;
    [SerializeField] protected Transform playerPosition;
    [SerializeField] protected Tilemap tileMap;
    private List<Vector3> nextMoves;

    void Start() {
        enemyDestination.parent = null;
        playerPosition = PlayerScript.Instance.getMovePoint();
        tileMap = GameObject.Find("Top").GetComponent<Tilemap>();
        nextMoves = new List<Vector3>();
    }

    void Update() {
        transform.position = Vector3.MoveTowards(transform.position, enemyDestination.position, enemySpeed * Time.deltaTime);
    }     

    public void EnemyMove() {
        // Find shortest path to player's new position
        nextMoves = AStar.FindPathClosest(tileMap, enemyDestination.position, playerPosition.position);

        if(nextMoves.Count > 1) {   // FindPathClosest returns current position as first element of the list
            Vector3 nextMove = nextMoves[1];
            Vector3 pathToPlayer = nextMove - enemyDestination.position;

            // Enemy only moves one unit in eight cardinal directions
            if (pathToPlayer.x != 0) {
                enemyDestination.position += pathToPlayer/Mathf.Abs(pathToPlayer.x);
            } else if (pathToPlayer.y != 0) {
                enemyDestination.position += pathToPlayer/Mathf.Abs(pathToPlayer.y);
            }
        }
    }
}
