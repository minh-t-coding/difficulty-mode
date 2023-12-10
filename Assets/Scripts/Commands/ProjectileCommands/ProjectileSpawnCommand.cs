using System.Transactions;
using UnityEditor.UI;
using UnityEngine;

public class ProjectileSpawnCommand : CommandManager.ICommand
{   
    GameObject projectile;
    GameObject projectileMovePoint;

    public string GetEntityType() { return "Projectile"; }
    
    public ProjectileSpawnCommand(GameObject projectile, GameObject projectileMovePoint) {
        this.projectile = projectile;
        this.projectileMovePoint = projectileMovePoint;
    }

    public virtual void Undo() {
        Object.Destroy(this.projectile);
        Object.Destroy(this.projectileMovePoint);
    }
}
