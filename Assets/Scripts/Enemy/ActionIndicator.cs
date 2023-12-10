using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class ActionIndicator : MonoBehaviour {
    // Start is called before the first frame update

    [SerializeField] protected TMP_Text textBox;


    protected Transform attachTo;

    protected Vector3 offset = new Vector3(0, 0.7f, -5);

    public enum ActionIndicatorActions {
        Attack,
        Move,
        Idle
    }

    public static ActionIndicator Create(Transform t) {
        GameObject indy = Instantiate(Resources.Load("Prefabs/ActionIndicator") as GameObject);
        ActionIndicator actionIndy = indy.GetComponent<ActionIndicator>();
        actionIndy.setAttachTo(t);
        actionIndy.SetIdleAction(new Vector3(0, 0, 0));
        return actionIndy;

    }
    // Update is called once per frame
    void Update() {
        transform.position = attachTo.position + offset;
    }

    public void SetAction(ActionIndicatorActions action, Vector3 dir) {
        switch (action) {
            case ActionIndicatorActions.Attack:
                SetAttackAction(dir);
                break;
            case ActionIndicatorActions.Move:
                SetMoveAction(dir);
                break;
            case ActionIndicatorActions.Idle:
                SetIdleAction(dir);
                break;
        }
    }
    public void SetMoveAction(Vector3 dir) {
        textBox.text = "";
    }

    public void SetAttackAction(Vector3 dir) {
        textBox.text = "!";
        offset = dir;
        textBox.color = new Color(1f, 0f, 0f, 1f);
    }

    public void SetIdleAction(Vector3 dir) {
        textBox.text = "";
    }

    public void setAttachTo(Transform a) {
        attachTo = a;
    }

}
