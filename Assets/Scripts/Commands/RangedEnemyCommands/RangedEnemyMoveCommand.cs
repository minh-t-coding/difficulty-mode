using System.Transactions;
using UnityEditor.UI;
using UnityEngine;

public class RangedEnemyMoveCommand : MoveCommand
{   
    RangedEnemyBehaviorScript enemyScript;

    public RangedEnemyMoveCommand(Vector3 movementDir, Vector3 prevOrientation, GameObject enemy) : base(movementDir, prevOrientation) {
        enemyScript = enemy.GetComponent<RangedEnemyBehaviorScript>();
    }

    public override void Undo() {
        enemyScript.MoveDestination(-1 * movementDir);
    }
}
