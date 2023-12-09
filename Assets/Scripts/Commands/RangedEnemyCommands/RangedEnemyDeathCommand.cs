using System.Transactions;
using UnityEditor.UI;
using UnityEngine;

public class RangedEnemyDeathCommand : DeathCommand
{   
    RangedEnemyBehaviorScript enemyScript;

    public RangedEnemyDeathCommand(Vector3 position, Vector3 orientation, RangedEnemyBehaviorScript enemyScript) : base(position, orientation) {
        this.enemyScript = enemyScript;
    }

    public override void Undo() {
        enemyScript.Spawn(position, orientation);
    }
}
