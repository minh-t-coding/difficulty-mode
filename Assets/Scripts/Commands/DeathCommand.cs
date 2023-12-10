using UnityEditor.UI;
using UnityEngine;

public class DeathCommand : CommandManager.ICommand
{
    protected Vector3 position;
    protected Vector3 orientation;

    public virtual string GetEntityType() { return ""; }
    
    public DeathCommand(Vector3 position, Vector3 orientation) {
        this.position = position;
        this.orientation = orientation;
    }

    public virtual void Undo() {
        
    }
}