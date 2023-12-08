using System.Transactions;
using UnityEditor.UI;
using UnityEngine;

public class RangedEnemyDeathCommand : DeathCommand
{   
    RangedEnemyBehaviorScript enemyScript;

    public RangedEnemyDeathCommand(Vector3 position, Vector3 orientation, GameObject enemy) : base(position, orientation) {
        enemyScript = enemy.GetComponent<RangedEnemyBehaviorScript>();
    }

    public override void Undo() {
        enemyScript.Spawn(position, orientation);
    }
}
