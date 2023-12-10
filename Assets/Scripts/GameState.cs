using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameState
{
    // Start is called before the first frame update

    protected List<GameObject> gameObjects;

    protected GameObject stateParent;

    protected PlayerState playerState;

    public PlayerState getPlayerState() {
        return playerState;
    }

    public void setPlayerState(PlayerState p) {
        playerState = p;
    }

    public void setGameObjects(List<GameObject> g) {
        gameObjects = g;
    }

    public List<GameObject> GetGameObjects() {
        return gameObjects;
    }

    public GameObject getStateParent() {
        return stateParent;
    }

    public void setStateParent(GameObject g) {
         stateParent = g;
    }

    


}
