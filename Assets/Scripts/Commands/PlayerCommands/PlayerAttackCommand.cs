using System.Transactions;
using System.Windows.Input;
using UnityEditor.UI;
using UnityEngine;

public class PlayerAttackCommand : CommandManager.ICommand
{   
    Vector3 attackDirection;
    PlayerBehaviorScript playerScript;

    public string GetEntityType() { return "Player"; }
    
    public PlayerAttackCommand(Vector3 attackDirection, PlayerBehaviorScript playerScript) {
        this.attackDirection = attackDirection;
        this.playerScript = playerScript;
    }

    public void Undo() {
        playerScript.undoPlayerAttack(attackDirection);
    }
}
