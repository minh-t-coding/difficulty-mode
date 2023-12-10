using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MEC;
using System.Linq;
public class BeatmapGenerator : MonoBehaviour
{   
    [SerializeField] protected float beatmapDelay;
    
    protected Beatmap beatmap;

    void Update() {
    }

    private KeyCode getDirectionKey(Vector3 direction) {
        if (direction == new Vector3(1, 1)) {
            return KeyCode.E;
        } else if (direction == new Vector3(1, -1)) {
            return KeyCode.C;
        } else if (direction == new Vector3(-1, 1)) {
            return KeyCode.Q;
        } else if (direction == new Vector3(-1, -1)) {
            return KeyCode.Z;
        } else if (direction == new Vector3(1, 0)) {
            return KeyCode.A;
        } else if (direction == new Vector3(-1, 0)) {
            return KeyCode.D;
        } else if (direction == new Vector3(0, 1)) {
            return KeyCode.W;
        } else if (direction == new Vector3(0, -1)) {
            return KeyCode.S;
        }

        return KeyCode.S;
    }

    public Beatmap GetBeatmap() {
        return beatmap;
    }

    public void GenerateBeatmap() {
        List<PlayerState> playerStates = GameStateManager.Instance.getPlayerStates();

        List<float> hits = new List<float>();
        List<KeyCode> keys = new List<KeyCode>();

        float curHit = 1;

        foreach (PlayerState state in playerStates) {
            PlayerState.PlayerAction action = state.getAction();
            Vector3 direction = state.getDirection();

            if (action == PlayerState.PlayerAction.Move) {
                keys.Add(getDirectionKey(direction));
                hits.Add(curHit);
                curHit += 1;
            } else if (action == PlayerState.PlayerAction.Dash) {
                keys.Add(getDirectionKey(direction));
                keys.Add(getDirectionKey(direction));
                hits.Add(curHit);
                hits.Add(curHit + (float)0.5);
                curHit += 1;
            } else if (action == PlayerState.PlayerAction.Attack) {
                keys.Add(KeyCode.Return);
                hits.Add(curHit);
                curHit += 1;
            }
        }

        beatmap = new Beatmap(hits.ToArray(), keys.ToArray());
    }
}
