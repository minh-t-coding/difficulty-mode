using System.Transactions;
using UnityEditor.UI;
using UnityEngine;

public class ProjectileDeathCommand : DeathCommand
{   
    GameObject projectilePrefab;
    Transform projectileManager;

    public ProjectileDeathCommand(Vector3 position, Vector3 orientation, GameObject projectilePrefab, Transform projectileManager) : base(position, orientation) {
        this.projectilePrefab = projectilePrefab;
        this.projectileManager = projectileManager;
    }

    public override void Undo() {
        int scriptDirection;

        if (orientation ==  new Vector3(0, 0, 0)) {
            scriptDirection = (int) PlayerBehaviorScript.Direction.Up;
        } else if (orientation == new Vector3(0, 0, 180)) {
            scriptDirection = (int) PlayerBehaviorScript.Direction.Down;
        } else if (orientation == new Vector3(0, 0, 90)) {
            scriptDirection = (int) PlayerBehaviorScript.Direction.Left;
        } else {
            scriptDirection = (int) PlayerBehaviorScript.Direction.Right;
        }
        
        GameObject projectile = UnityEngine.Object.Instantiate(projectilePrefab, position, Quaternion.Euler(orientation), projectileManager);
        projectile.GetComponent<ProjectileBehaviorScript>().setDirection(scriptDirection);
    }
}
