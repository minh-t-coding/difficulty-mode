using System.Transactions;
using UnityEditor.UI;
using UnityEngine;

public class PlayerMoveCommand : MoveCommand
{   
    PlayerBehaviorScript playerScript;

    public override string GetEntityType() { return "Player"; }
    
    public PlayerMoveCommand(Vector3 movementDir, Vector3 prevOrientation, PlayerBehaviorScript playerScript) : base(movementDir, prevOrientation) {
        this.playerScript = playerScript;
    }

    public override void Undo() {
        playerScript.MoveDestination(-1 * movementDir);
    }
}
