using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class EnemiesRemainingScript : MonoBehaviour {

    [SerializeField] protected TextMeshProUGUI rangedEnemyCountText;
    [SerializeField] protected TextMeshProUGUI meleeEnemyCountText;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Tuple<int, int> enemiesCount = EnemyManagerScript.Instance.getEnemyCounts();
        meleeEnemyCountText.text = "x" + enemiesCount.Item1.ToString();
        rangedEnemyCountText.text = "x" + enemiesCount.Item2.ToString();
    }
}
