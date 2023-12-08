using System.Transactions;
using System.Windows.Input;
using UnityEditor.UI;
using UnityEngine;

public class PlayerAttackCommand : CommandManager.ICommand
{   
    Vector3 attackDirection;
    PlayerBehaviorScript playerScript;

    public PlayerAttackCommand(Vector3 attackDirection, GameObject player) {
        playerScript = player.GetComponent<PlayerBehaviorScript>();
    }

    public void Undo() {
        playerScript.undoPlayerAttack(attackDirection);
    }
}
