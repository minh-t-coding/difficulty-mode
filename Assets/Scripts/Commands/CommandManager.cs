using System.Collections.Generic;
using UnityEngine;

public class CommandManager : MonoBehaviour {
    public interface ICommand {
        void Execute();
        void Undo();
    }

    public static CommandManager Instance;

    private Stack<ICommand> commandsBuffer = new Stack<ICommand>();

    private void Awake() {
        if (Instance == null) {
            Instance = this;
        }
    }

    public void AddCommand(ICommand command) {
        command.Execute();
        commandsBuffer.Push(command);
    }

    public void Undo() {
        if (commandsBuffer.Count == 0) {
            return;
        }
        ICommand command = commandsBuffer.Pop();
        command.Undo();
    }
}
