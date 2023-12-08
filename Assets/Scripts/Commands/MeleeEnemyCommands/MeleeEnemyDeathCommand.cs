using System.Transactions;
using UnityEditor.UI;
using UnityEngine;

public class MeleeEnemyDeathCommand : DeathCommand
{   
    MeleeEnemyBehaviorScript enemyScript;

    public MeleeEnemyDeathCommand(Vector3 position, Vector3 orientation, GameObject enemy) : base(position, orientation) {
        enemyScript = enemy.GetComponent<MeleeEnemyBehaviorScript>();
    }

    public override void Undo() {
        enemyScript.Spawn(position, orientation);
    }
}
