using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeadEnemy : StateEntity
{
    public override void OnStateLoad() {
        transform.parent = null;
    }
}
