using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInputManager : MonoBehaviour {
    // Start is called before the first frame update
    public static PlayerInputManager Instance;

    protected bool isStickoMode;


    [SerializeField] protected List<KeyCode> inputMapping;
    public enum PlayerInputActions {
        MoveRight,
        MoveUp,
        MoveLeft,
        MoveDown,
        Attack,
        NONE
    }

    protected List<PlayerInputActions> allowedActions;


    public PlayerInputActions getActionMappedToKey(KeyCode key) {
        for (int i = 0; i < inputMapping.Count; i++) {
            if (inputMapping[i] == key) {
                return ((PlayerInputActions)i);
            }
        }
        return PlayerInputActions.NONE;

    }

    public bool getIsStickoMode() {
        return isStickoMode;
    }

    public KeyCode getKeyCodeMappedToAction(PlayerInputActions action) {
        int i = (int)action;
        if (i >= 0 && i < inputMapping.Count) {
            return inputMapping[i];
        }
        return KeyCode.None;
    }

    void Awake() {
        if (Instance == null) {
            Instance = this;

        }
    }

    public void setIsStickoMode(bool b) {
        allowedActions = new List<PlayerInputActions>();
        isStickoMode = b;
    }

    public Vector3 getDirectionalInput() {
        if (isStickoMode) {
            Vector3 retVec = new Vector3(0, 0, 0);
            if (allowedActions.Contains(PlayerInputActions.MoveRight)) {
                if (Input.GetKey(getKeyCodeMappedToAction(PlayerInputActions.MoveRight))) {
                    retVec += new Vector3(1, 0, 0);
                }
            }
            if (allowedActions.Contains(PlayerInputActions.MoveUp)) {
                if (Input.GetKey(getKeyCodeMappedToAction(PlayerInputActions.MoveUp))) {
                    retVec += new Vector3(0, 1, 0);
                }
            }
            if (allowedActions.Contains(PlayerInputActions.MoveLeft)) {
                if (Input.GetKey(getKeyCodeMappedToAction(PlayerInputActions.MoveLeft))) {
                    retVec += new Vector3(-1, 0, 0);
                }
            }
            if (allowedActions.Contains(PlayerInputActions.MoveDown)) {
                if (Input.GetKey(getKeyCodeMappedToAction(PlayerInputActions.MoveDown))) {
                    retVec += new Vector3(0, -1, 0);
                }
            }
            return retVec;
        }
        return new Vector3(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"), 0f);
    }

    public bool getAttackInput() {
        if (isStickoMode) {
            if (allowedActions.Contains(PlayerInputActions.Attack)) {
                return Input.GetKey(KeyCode.Return);
            }
            else {
                return false;
            }
        }
        return Input.GetKey(KeyCode.Return);
    }

    public void setAllowedActions(List<KeyCode> presses) {
        allowedActions = new List<PlayerInputActions>();
        foreach (KeyCode key in presses) {
            //Debug.Log(getActionMappedToKey(key).ToString());
            allowedActions.Add(getActionMappedToKey(key));
        }
    }


    // Update is called once per frame
    void Update() {

    }
}
