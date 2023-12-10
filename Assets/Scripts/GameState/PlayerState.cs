using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerState {
    protected Vector3 pos;

    protected string action;

    protected bool isDead;

    public PlayerState(Vector3 p, string a) {
        pos = p;
        action = a;
        isDead = false;
    }

    public string getAction() {
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
