using System;
using TMPro;
using UnityEngine;

public class EnemiesRemainingScript : MonoBehaviour
{
    [SerializeField] protected TextMeshProUGUI rangedEnemyCountText;
    [SerializeField] protected TextMeshProUGUI meleeEnemyCountText;
 
    // Update is called once per frame
    void Update()
    {
        Tuple<int, int> enemiesCount = EnemyManagerScript.Instance.getEnemyCounts();
        meleeEnemyCountText.text = "x" + enemiesCount.Item1.ToString();
        rangedEnemyCountText.text = "x" + enemiesCount.Item2.ToString();
    }
}
