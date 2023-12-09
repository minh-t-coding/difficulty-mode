using System;
using System.Collections.Generic;
using UnityEngine;

public class CommandManager : MonoBehaviour {
    public interface ICommand {
        void Undo();
    }

    public static CommandManager Instance;

    private int maxStackCount;
    private Dictionary<int, Stack<List<ICommand>>> stackDict = new Dictionary<int, Stack<List<ICommand>>>();

    private void Awake() {
        if (Instance == null) {
            Instance = this;
        }
    }

    public void AddCommand(int instanceId, List<ICommand> command) {
        Debug.Log("adding command for " + instanceId);
        Debug.Log(command);
        
        if (stackDict.ContainsKey(instanceId)) {
            stackDict[instanceId].Push(command);
        } else {
            Stack<List<ICommand>> newStack = new Stack<List<ICommand>>();
            newStack.Push(command);
            stackDict.Add(instanceId, newStack);
        }

        maxStackCount = Math.Max(maxStackCount, stackDict[instanceId].Count);
    }

    public void Undo() {
        // go through all of keys and pop if length is max stack count
        foreach (KeyValuePair<int, Stack<List<ICommand>>> pair in stackDict)
        {
            if (pair.Value.Count == maxStackCount) {
                List<ICommand> commandList = pair.Value.Pop();

                foreach (ICommand command in commandList) {
                    command.Undo();
                }
            }
        }

        maxStackCount -= 1;
    }
}
