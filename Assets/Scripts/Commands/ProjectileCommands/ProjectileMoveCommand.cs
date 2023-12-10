using System.Transactions;
using UnityEditor.UI;
using UnityEngine;

public class ProjectileMoveCommand : MoveCommand
{   
    ProjectileBehaviorScript projectileScript;

    public override string GetEntityType() { return "Projectile"; }
    
    public ProjectileMoveCommand(Vector3 movementDir, Vector3 prevOrientation, ProjectileBehaviorScript projectileScript) : base(movementDir, prevOrientation) {
        this.projectileScript = projectileScript;
    }

    public override void Undo() {
        projectileScript.MoveDestination(-1 * movementDir);
    }
}
