using System.Transactions;
using UnityEditor.UI;
using UnityEngine;

public class RangedEnemyAttackCommand : CommandManager.ICommand
{   
    Vector3 attackDirection;
    RangedEnemyBehaviorScript enemyScript;

    public RangedEnemyAttackCommand(Vector3 attackDirection, GameObject enemy) {
        this.attackDirection = attackDirection;
        enemyScript = enemy.GetComponent<RangedEnemyBehaviorScript>();
    }

    public virtual void Undo() {
        enemyScript.EnemyUndoAttack(attackDirection);
    }
}
