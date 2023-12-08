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

    public virtual void EnemyMove() {}
}