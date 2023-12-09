using System.Collections;
using System.Collections.Generic;
using Toolbox;
using UnityEngine;
using UnityEngine.Tilemaps;

public class MeleeEnemyBehaviorScript : BaseEnemyBehavior {
    public override void EnemyMove() {
        base.EnemyMove();

        if (isMoving) {
            // add movement command
            List<CommandManager.ICommand> commandList = new List<CommandManager.ICommand>
            {
                new MeleeEnemyMoveCommand(movePath, new Vector3(0, 0, 0), this)
            };
            CommandManager.Instance.AddCommand(this.GetInstanceID(), commandList);
        }
    }

    public override void EnemyAttacked(float damage)
    {
        base.EnemyAttacked(damage);

        // add melee enemy death command
        if (enemyHealth <= 0) {
            List<CommandManager.ICommand> commandList = new List<CommandManager.ICommand>
            {
                new MeleeEnemyDeathCommand(enemyDestination.position, Vector3.zero, this)
            };
            CommandManager.Instance.AddCommand(this.GetInstanceID(), commandList);
        }
    }

    public override void EnemyAttack()
    {
        base.EnemyAttack();

        // add player attack command
        List<CommandManager.ICommand> commandList = new List<CommandManager.ICommand>
        {
            new MeleeEnemyAttackCommand(new Vector3(0, 0, 0), this)
        };
        CommandManager.Instance.AddCommand(this.GetInstanceID(), commandList);
    }
}