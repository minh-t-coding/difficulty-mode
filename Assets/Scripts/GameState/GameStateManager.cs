using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameStateManager : MonoBehaviour {
    public static GameStateManager Instance;
    protected List<GameState> gameStates;

    protected int currTurn;

    protected bool savedInitState = false;
    void Awake() {
        if (Instance == null) {
            Instance = this;
            gameStates = new List<GameState>();
        }
    }

    public void captureGameState() {
        isBusy = true;
        clearGameState(currTurn);
        GameObject newStateParent = new GameObject("GameState" + currTurn);
        Debug.Log("CAPTURE STATE" + currTurn);
        newStateParent.transform.parent = transform;
        GameState newState = new GameState();
        newState.setPlayerState(PlayerBehaviorScript.Instance.GetPlayerState());
        //Debug.Log(PlayerBehaviorScript.Instance.GetPlayerState().getAction());
        StateEntity[] entities = GameObject.FindObjectsOfType<StateEntity>(false);
        List<GameObject> savedObjects = new List<GameObject>();
        foreach (StateEntity entity in entities) {
            GameObject copy = Instantiate(entity.gameObject);
            ProjectileBehaviorScript proj = entity.gameObject.GetComponent<ProjectileBehaviorScript>();
            if (proj != null) {
                proj.copyDir(copy.gameObject.GetComponent<ProjectileBehaviorScript>());
            }
            BaseEnemy enem = entity.gameObject.GetComponent<BaseEnemy>();
            if (enem != null) {
                enem.copyEnemy(copy.gameObject.GetComponent<BaseEnemy>());
            }
            copy.name = entity.gameObject.name;
            copy.SetActive(false);
            copy.transform.parent = newStateParent.transform;
            savedObjects.Add(copy);
        }
        newState.setStateParent(newStateParent);
        newState.setGameObjects(savedObjects);

        gameStates.Add(newState);
        currTurn++;
        isBusy=false;
    }

    public int getNumDeadEnemiesInState(int i) {
        int cnt = 0;
        if (gameStates.Count > i && i >= 0) {
            List<GameObject> savedObjects = gameStates[i].GetGameObjects();
            foreach(GameObject g in savedObjects) {
                if (g.name.Contains("EnemyDead")) {
                    cnt++;
                }
            }
        }
        //Debug.Log(cnt);
        return cnt;
    }

    public void clearGameState(int turn) {
        if (gameStates.Count > turn && turn >= 1) {
            GameState state = gameStates[turn];
            Destroy(state.getStateParent());
            gameStates.Remove(state);

        }
    }

    public void unloadCurrState() {
        StateEntity[] entities = GameObject.FindObjectsOfType<StateEntity>(false);
        foreach (StateEntity entity in entities) {
            entity.DestroyAssociates();
            Destroy(entity.gameObject);
        }
    }
    protected bool isBusy;


    public void loadGameState(int turn, bool clearAllAheadStates = true, bool dontUnload = false) {
        isBusy = true;
        if (currTurn == turn) {
            Debug.Log("Tried to load same state?");
            return;
        }
        if (gameStates.Count > turn && turn >= 1) {

            unloadCurrState();
            Debug.Log("LOADING STATE" + turn);
            GameState state = gameStates[turn];
            PlayerBehaviorScript.Instance.gameObject.SetActive(true);
            PlayerBehaviorScript.Instance.LoadPlayerState(state.getPlayerState());
            List<GameObject> savedObjects = state.GetGameObjects();
            foreach (GameObject obj in savedObjects) {
                if (dontUnload) {
                    GameObject newCopy = Instantiate(obj);
                    ProjectileBehaviorScript proj = obj.gameObject.GetComponent<ProjectileBehaviorScript>();
                    if (proj != null) {
                        proj.copyDir(newCopy.gameObject.GetComponent<ProjectileBehaviorScript>());
                    }
                    BaseEnemy enem = obj.gameObject.GetComponent<BaseEnemy>();
                    if (enem != null) {
                        enem.copyEnemy(newCopy.gameObject.GetComponent<BaseEnemy>());
                    }
                    newCopy.SetActive(true);
                    newCopy.GetComponent<StateEntity>().OnStateLoad();
                } else {
                    obj.SetActive(true);
                    obj.GetComponent<StateEntity>().OnStateLoad();
                }
                
            }

            if (clearAllAheadStates) {
                for (int i = turn; i < gameStates.Count; i++) {
                    clearGameState(i);
                }
            } 
            
            currTurn = turn;
        }
        
        isBusy = false;
    }

    public int GetCurrTurn() {
        return this.currTurn;
    }

    public List<PlayerState> getPlayerStates() {
        List<PlayerState> playerStates = new List<PlayerState>();
        Debug.Log("PLAYER STATES!!");
        foreach (GameState state in gameStates) {
            //Debug.Log(state.getPlayerState().getAction().ToString() +state.getPlayerState().getDirection());
            playerStates.Add(state.getPlayerState());
        }
        Debug.Log("DONEE!!");
        return playerStates;
    }

    // Update is called once per frame
    void Update() {

        if (!savedInitState) {
            savedInitState = true;
            captureGameState();
        }
        if (Input.GetKeyDown(KeyCode.Backspace) && !isBusy && !PlayerInputManager.Instance.getIsStickoMode()) {
            loadGameState(currTurn - 1);
        }
    }

}
