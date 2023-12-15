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
        Dash,
        NONE
    }

    public bool isAttackAllowed() {
        return allowedActions.Contains(PlayerInputActions.Attack);
    }

    protected List<PlayerInputActions> allowedActions;

    protected Dictionary<KeyCode, bool> needsToLiftDict;


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
            needsToLiftDict = new Dictionary<KeyCode, bool>();
            foreach(KeyCode key in inputMapping) {
                needsToLiftDict.Add(key, false);
            }
        }
    }

    public void setIsStickoMode(bool b) {
        allowedActions = new List<PlayerInputActions>();
        isStickoMode = b;
    }

    public bool pressedAllDirectionals() {
        if (getGoalDirectionalInput() == new Vector3(0,0,0)) {
            return false;
        }
        return getGoalDirectionalInput() == getDirectionalInput();
    }

    public bool pressedWrongInput(List<KeyCode> currHits) {
        bool wrongInput = false;
        foreach(KeyCode key in inputMapping) {
            if (!currHits.Contains(key)) {
                if (Input.GetKey(key)) {
                    //Debug.Log("MISSED" + key);
                }
                wrongInput = wrongInput || Input.GetKey(key);
            }
        }
        return wrongInput;
    }

    public bool getNeedsToLift(KeyCode key) {
        return  needsToLiftDict[key];
    }

    public bool pressedNoDirectionals() {
        return getDirectionalInput() == new Vector3(0,0,0);
    }

    public Vector3 getGoalDirectionalInput() {
        Vector3 retVec = new Vector3(0, 0, 0);
        if (allowedActions.Contains(PlayerInputActions.MoveRight)) {
            retVec += new Vector3(1, 0, 0);
        }
        if (allowedActions.Contains(PlayerInputActions.MoveUp)) {
            retVec += new Vector3(0, 1, 0);
        }
        if (allowedActions.Contains(PlayerInputActions.MoveLeft)) {
            retVec += new Vector3(-1, 0, 0);
        }
        if (allowedActions.Contains(PlayerInputActions.MoveDown)) {
            retVec += new Vector3(0, -1, 0);
        }
        return retVec;
    }

    public Vector3 getDirectionalInput() {
        
        Vector3 retVec = new Vector3(0, 0, 0);
        if (!isStickoMode || (allowedActions.Contains(PlayerInputActions.MoveRight))) {
            if (Input.GetKey(getKeyCodeMappedToAction(PlayerInputActions.MoveRight))) {
                retVec += new Vector3(1, 0, 0);
            }
        }
        if (!isStickoMode || (allowedActions.Contains(PlayerInputActions.MoveUp) )) {
            if (Input.GetKey(getKeyCodeMappedToAction(PlayerInputActions.MoveUp))) {
                retVec += new Vector3(0, 1, 0);
            }
        }
        if (!isStickoMode || (allowedActions.Contains(PlayerInputActions.MoveLeft))) {
            if (Input.GetKey(getKeyCodeMappedToAction(PlayerInputActions.MoveLeft))) {
                retVec += new Vector3(-1, 0, 0);
            }
        }
        if (!isStickoMode || (allowedActions.Contains(PlayerInputActions.MoveDown) )) {
            if (Input.GetKey(getKeyCodeMappedToAction(PlayerInputActions.MoveDown))) {
                retVec += new Vector3(0, -1, 0);
            }
        }
        return retVec;
        
        //return new Vector3(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"), 0f);
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

    public void setAllowedActions(List<KeyCode> presses, bool isDash) {
        allowedActions = new List<PlayerInputActions>();
        if (presses.Count==0) {
            //Debug.Log("EMPTYY!");
        }
        foreach (KeyCode key in presses) {
            //Debug.Log(getActionMappedToKey(key).ToString());
            allowedActions.Add(getActionMappedToKey(key));
        }
        if (isDash) {
             allowedActions.Add(PlayerInputActions.Dash);
        }
    }

    public List<PlayerInputActions> getAllowedActions() {
        return allowedActions;
    }

    public void setNeedsToLift(KeyCode key) {
        needsToLiftDict[key] = true;
    }

    // Update is called once per frame
    void Update() {
        foreach(KeyCode key in inputMapping) {
            if (!Input.GetKey(key) && needsToLiftDict[key]) {
                needsToLiftDict[key] = false;
            }
        }
    }
}
