using System.Transactions;
using UnityEditor.UI;
using UnityEngine;

public class PlayerDeathCommand : DeathCommand
{   
    PlayerBehaviorScript playerScript;

    public PlayerDeathCommand(Vector3 position, Vector3 orientation, GameObject player) : base(position, orientation) {
        playerScript = player.GetComponent<PlayerBehaviorScript>();
    }

    public override void Undo() {
        playerScript.Spawn(position, orientation);
    }
}
