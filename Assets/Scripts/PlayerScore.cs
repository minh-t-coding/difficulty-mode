using System;
using TMPro;
using UnityEngine;

public class PlayerScore : MonoBehaviour
{
    [SerializeField] protected TextMeshProUGUI scoreValueText;

    void Start() {
    }

    void Update()
    {
        int playerScore = ScoreManagerScript.Instance.GetPlayerScore();
        scoreValueText.text = playerScore.ToString();
    }
}
