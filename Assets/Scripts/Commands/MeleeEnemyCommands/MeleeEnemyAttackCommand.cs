using System.Transactions;
using UnityEditor.UI;
using UnityEngine;

public class MeleeEnemyAttackCommand : CommandManager.ICommand
{   
    Vector3 attackDirection;
    MeleeEnemyBehaviorScript enemyScript;

    public MeleeEnemyAttackCommand(Vector3 attackDirection, GameObject enemy) {
        this.attackDirection = attackDirection;
        enemyScript = enemy.GetComponent<MeleeEnemyBehaviorScript>();
    }

    public virtual void Undo() {
        enemyScript.EnemyUndoAttack(attackDirection);
    }
}
