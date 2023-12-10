using System.Transactions;
using UnityEditor.UI;
using UnityEngine;

public class MeleeEnemyMoveCommand : MoveCommand
{   
    // TODO: see if we can change this to MeleeEnemyBehavior instead of BaseEnemyBehavior
    MeleeEnemyBehaviorScript enemyScript;

    public override string GetEntityType() { return "MeleeEnemy"; }
    
    public MeleeEnemyMoveCommand(Vector3 movementDir, Vector3 prevOrientation, MeleeEnemyBehaviorScript enemyScript) : base(movementDir, prevOrientation) {
        this.enemyScript = enemyScript;
    }

    public override void Undo() {
        enemyScript.MoveDestination(-1 * movementDir);
    }
}
