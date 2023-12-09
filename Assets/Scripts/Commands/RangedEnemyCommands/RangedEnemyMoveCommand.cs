using System.Transactions;
using UnityEditor.UI;
using UnityEngine;

public class RangedEnemyMoveCommand : MoveCommand
{   
    RangedEnemyBehaviorScript enemyScript;

    public RangedEnemyMoveCommand(Vector3 movementDir, Vector3 prevOrientation, RangedEnemyBehaviorScript enemyScript) : base(movementDir, prevOrientation) {
        this.enemyScript = enemyScript;
    }

    public override void Undo() {
        enemyScript.MoveDestination(-1 * movementDir);
    }
}
