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
    private List<Vector3> nextMove;

    void Start() {
        enemyDestination.parent = null;
        playerPosition = PlayerScript.Instance.getMovePoint();
        tileMap = GameObject.Find("Ground").GetComponent<Tilemap>();
        nextMove = new List<Vector3>();
    }

    void Update() {
        transform.position = Vector3.MoveTowards(transform.position, enemyDestination.position, enemySpeed * Time.deltaTime);
    }

    public void EnemyMove() {
        nextMove = AStar.FindPathClosest(tileMap, enemyDestination.position, playerPosition.position);
        Debug.Log("nextMove: " + nextMove); // nextMove is null

        // Vector3 pathToPlayer = playerPosition.position - enemyDestination.position;

        // if (pathToPlayer.x > 0) {
        //     enemyDestination.position += new Vector3(1f, 0f, 0f);
        // } else if (pathToPlayer.x < 0) {
        //     enemyDestination.position += new Vector3(-1f, 0f, 0f);
        // }

        // if (pathToPlayer.y > 0) {
        //     enemyDestination.position += new Vector3(0f, 1f, 0f);
        // } else if (pathToPlayer.y < 0) {
        //     enemyDestination.position += new Vector3(0f, -1f, 0f);
        // }
    }
}
