using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateEntity : MonoBehaviour {

    protected bool createdAssociates;
    public virtual void OnStateLoad() {

    }

    public virtual void CreateAssociates() {

    }


    public virtual void DestroyAssociates() {
        Debug.Log("DESTROY VBLANKK");
    }
}
