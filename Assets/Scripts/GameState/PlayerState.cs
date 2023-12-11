using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerState {
    public enum PlayerAction {
        Move,
        Dash,
        Attack
    }
    
    protected Vector3 pos;

    protected PlayerAction action;
    protected Vector3 direction;

    protected bool isDead;

    public PlayerState(Vector3 p, Vector3 d, PlayerAction a) {
        Debug.Log("direction " + d);
        pos = p;
        direction = d;
        action = a;
        isDead = false;
    }

    public Vector3 getDirection() {
        return direction;
    }

    public PlayerAction getAction() {
        return action;
    }

    public void setPos(Vector3 v) {
        pos = v;

    }

    public void setIsDead(bool b) {
        isDead = b;
    }

    public bool getIsDead() {
        return isDead;
    }

    public Vector3 getPos() {
        return pos;
    }
}
