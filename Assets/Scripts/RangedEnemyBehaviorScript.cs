using System.Collections;
using System.Collections.Generic;
using UnityEngine;

enum Direction {
    UP,
    DOWN,
    LEFT,
    RIGHT
}

public class RangedEnemyBehaviorScript : MonoBehaviour {
    // Gun Variables
    [SerializeField] protected Transform shootingPoint;
    [SerializeField] protected GameObject projectilePrefab;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.I)) {
            Shoot(Direction.UP);
        }
        if (Input.GetKeyDown(KeyCode.K)) {
            Shoot(Direction.DOWN);
        }
        if (Input.GetKeyDown(KeyCode.J)) {
            Shoot(Direction.LEFT);
        }
        if (Input.GetKeyDown(KeyCode.L)) {
            Shoot(Direction.RIGHT);
        }
    }

    private void Shoot(Direction direction) {
        switch (direction) {
            case Direction.UP:
                Instantiate(projectilePrefab, shootingPoint.position, Quaternion.Euler(new Vector3(0, 0, 0)));
                break;
            case Direction.DOWN:
                Instantiate(projectilePrefab, shootingPoint.position, Quaternion.Euler(new Vector3(0, 0, 180)));
                break;
            case Direction.LEFT:
                Instantiate(projectilePrefab, shootingPoint.position, Quaternion.Euler(new Vector3(0, 0, 90)));
                break;
            case Direction.RIGHT:
                Instantiate(projectilePrefab, shootingPoint.position, Quaternion.Euler(new Vector3(0, 0, -90)));
                break;
        }
        
    }
}
