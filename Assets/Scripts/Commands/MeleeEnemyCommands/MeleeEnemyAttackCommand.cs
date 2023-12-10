using System;
using System.Transactions;
using UnityEditor.UI;
using UnityEngine;

public class MeleeEnemyAttackCommand : CommandManager.ICommand
{   
    Vector3 attackDirection;
    MeleeEnemyBehaviorScript enemyScript;

    public string GetEntityType() { return "MeleeEnemy"; }

    public MeleeEnemyAttackCommand(Vector3 attackDirection, MeleeEnemyBehaviorScript enemyScript) {
        this.attackDirection = attackDirection;
        this.enemyScript = enemyScript;
    }

    public virtual void Undo() {
        enemyScript.EnemyUndoAttack(attackDirection);
    }
}
