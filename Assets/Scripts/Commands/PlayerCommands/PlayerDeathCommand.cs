using System.Transactions;
using UnityEditor.UI;
using UnityEngine;

public class PlayerDeathCommand : DeathCommand
{   
    PlayerBehaviorScript playerScript;

    public PlayerDeathCommand(Vector3 position, Vector3 orientation, PlayerBehaviorScript playerScript) : base(position, orientation) {
        this.playerScript = playerScript;
    }

    public override void Undo() {
        playerScript.Spawn(position, orientation);
    }
}
