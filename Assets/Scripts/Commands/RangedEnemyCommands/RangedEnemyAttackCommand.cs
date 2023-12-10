using System.Transactions;
using UnityEditor.UI;
using UnityEngine;

public class RangedEnemyAttackCommand : CommandManager.ICommand
{   
    Vector3 attackDirection;
    RangedEnemyBehaviorScript enemyScript;

    public string GetEntityType() { return "RangedEnemy"; }
    
    public RangedEnemyAttackCommand(Vector3 attackDirection, RangedEnemyBehaviorScript enemyScript) {
        this.attackDirection = attackDirection;
        this.enemyScript = enemyScript;
    }

    public virtual void Undo() {
        enemyScript.EnemyUndoAttack(attackDirection);
    }
}
