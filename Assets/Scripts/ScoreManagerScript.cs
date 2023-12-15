using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreManagerScript : MonoBehaviour
{
    public static ScoreManagerScript Instance;

    [SerializeField] protected int parTurnCount;
    [SerializeField] protected int perfectHitScore = 500;
    [SerializeField] protected int greatHitScore = 300;
    [SerializeField] protected int goodHitScore = 100;
    
    protected int turnCountMultipler = 100;
    protected int hitBeat;
    
    protected int playerScore;
    
    void Awake() {
        if (Instance == null) {
            Instance = this;
        }
    }
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void UpdateScoreBeatHit(float diff) {
        if (diff < 0.1) {
            playerScore += perfectHitScore;
        } else if (diff < 0.15) {
            playerScore += greatHitScore;
        } else {
            playerScore += goodHitScore;
        }
    }

    public void UpdateScoreBeatMiss() {
        playerScore -= 500;
    }

    public int GetPlayerScore() {
        return playerScore;
    }
}
