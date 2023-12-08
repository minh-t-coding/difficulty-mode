using System.Transactions;
using UnityEditor.UI;
using UnityEngine;

public class ProjectileMoveCommand : MoveCommand
{   
    ProjectileBehaviorScript projectileScript;

    public ProjectileMoveCommand(Vector3 movementDir, Vector3 prevOrientation, GameObject projectile) : base(movementDir, prevOrientation) {
        this.projectileScript = projectile.GetComponent<ProjectileBehaviorScript>();
    }

    public override void Undo() {
        projectileScript.MoveDestination(-1 * movementDir);
    }
}
