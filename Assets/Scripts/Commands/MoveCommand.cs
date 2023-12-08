using UnityEditor.UI;
using UnityEngine;

public class MoveCommand : CommandManager.ICommand
{
    protected Vector3 movementDir;
    protected Vector3 prevOrientation;
    
    public MoveCommand(Vector3 movementDir, Vector3 prevOrientation) {
        this.movementDir = movementDir;
        this.prevOrientation = prevOrientation;
    }

    public virtual void Undo() {
        
    }
}