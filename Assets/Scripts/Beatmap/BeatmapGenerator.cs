using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MEC;
using System.Linq;
using UnityEngine.AI;
using UnityEditor.Experimental.GraphView;
using System.Security.Cryptography;
public class BeatmapGenerator : MonoBehaviour
{   
    [SerializeField] protected float beatmapDelay;
    
    protected Beatmap beatmap;

    public static BeatmapGenerator Instance;

    void Awake() {
        if (Instance==null) {
            Instance = this;
        }
    } 

    // can return multiple keys in the case of diagonal movements
    private List<KeyCode> getDirectionKeys(Vector3 direction) {
        List<KeyCode> keys = new List<KeyCode>();
        
        if (direction.x == 1f) {
            keys.Add(KeyCode.D);
        } else if (direction.x == -1f) {
            keys.Add(KeyCode.A);
        }
        if (direction.y == 1f) {
            keys.Add(KeyCode.W);
        } else if (direction.y == -1f) {
            keys.Add(KeyCode.S);
        }

        return keys;
    }

    public Beatmap GetBeatmap() {
        return beatmap;
    }

    public void GenerateBeatmap() {
        List<PlayerState> playerStates = GameStateManager.Instance.getPlayerStates();

        List<float> hits = new List<float>();
        List<KeyCode> keys = new List<KeyCode>();
        List<PlayerState.PlayerAction> actions = new List<PlayerState.PlayerAction>();

        float curHit = 0;
        int currState = 0;
        foreach (PlayerState state in playerStates) {
            if (currState==0 || currState == playerStates.Count-1) {
                currState++;
                continue;
            }
            PlayerState.PlayerAction action = state.getAction();
            
            //Debug.Log(action.ToString());
            Vector3 direction = state.getDirection();

            if (action == PlayerState.PlayerAction.Move) {
                List<KeyCode> keyCodes = getDirectionKeys(direction);
                
                foreach(KeyCode key in keyCodes) {
                    keys.Add(key);
                    hits.Add(curHit);
                    actions.Add(action);
                }

                curHit += 1;
            } else if (action == PlayerState.PlayerAction.Dash) {
                List<KeyCode> keyCodes = getDirectionKeys(direction);
                
                foreach(KeyCode key in keyCodes) {
                    keys.Add(key);
                    hits.Add(curHit);
                    actions.Add(action);
                }

                foreach(KeyCode key in keyCodes) {
                    keys.Add(key);
                    hits.Add(curHit + 0.5f);
                    actions.Add(action);
                }

                curHit += 1;
            } else if (action == PlayerState.PlayerAction.Attack) {
                keys.Add(KeyCode.Return);
                hits.Add(curHit);
                actions.Add(action);
                List<KeyCode> keyCodes = getDirectionKeys(direction);
                foreach(KeyCode key in keyCodes) {
                    keys.Add(key);
                    hits.Add(curHit);
                    actions.Add(action);
                }

                curHit += 1;
            }
            currState++;
        }
        beatmap = new Beatmap(hits.ToArray(), keys.ToArray(),actions.ToArray(),4f);
    }
}
