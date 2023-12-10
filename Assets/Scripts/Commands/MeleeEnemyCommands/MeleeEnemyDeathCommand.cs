using System.Transactions;
using UnityEditor.UI;
using UnityEngine;

public class MeleeEnemyDeathCommand : DeathCommand
{   
    MeleeEnemyBehaviorScript enemyScript;

    public override string GetEntityType() { return "MeleeEnemy"; }

    public MeleeEnemyDeathCommand(Vector3 position, Vector3 orientation, MeleeEnemyBehaviorScript enemyScript) : base(position, orientation) {
        this.enemyScript = enemyScript;
    }

    public override void Undo() {
        enemyScript.Spawn(position, orientation);
    }
}
