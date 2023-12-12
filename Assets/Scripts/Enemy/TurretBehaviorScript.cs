using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PlayerLoop;

public class TurretBehaviorScript : MonoBehaviour {

    enum Direction { Up, Down, Left, Right }
    [SerializeField] Direction shootingDirection;
    [SerializeField] protected GameObject projectilePrefab;

    public void TurretAttack() {
        Vector3 scriptDirection;

        if (shootingDirection == Direction.Up) {
            scriptDirection = new Vector3(0, 1, 0);
        } else if (shootingDirection == Direction.Down) {
            scriptDirection = new Vector3(0, -1, 0);
        } else if (shootingDirection == Direction.Left) {
            scriptDirection = new Vector3(-1, 0, 0);
        } else {
            scriptDirection = new Vector3(1, 0, 0);
        }

        GameObject projectile = Instantiate(projectilePrefab, transform.position, Quaternion.identity, ProjectileManagerScript.Instance.transform);
        projectile.GetComponent<ProjectileBehaviorScript>().OnStateLoad();
        projectile.GetComponent<ProjectileBehaviorScript>().setDirection(scriptDirection.normalized);
    }
}
