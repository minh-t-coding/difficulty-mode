using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CommandManager : MonoBehaviour {
    public interface ICommand {
        string GetEntityType();
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

    void Update() {
    }


    public void AddCommand(int instanceId, List<ICommand> commands) {
        Debug.Log("Adding command.");
        
        foreach(ICommand command in commands) {
            if (command.GetEntityType() == "Projectile") {
                Debug.Log(instanceId);
                Debug.Log(command);
            }
        }
        
        if (stackDict.ContainsKey(instanceId)) {
            stackDict[instanceId].Push(commands);
        } else {
            Stack<List<ICommand>> newStack = new Stack<List<ICommand>>();
            newStack.Push(commands);
            stackDict.Add(instanceId, newStack);
        }

        maxStackCount = Math.Max(maxStackCount, stackDict[instanceId].Count);
    }

    public void Undo() {
        if (maxStackCount == 0) {
            return;
        }
        
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
