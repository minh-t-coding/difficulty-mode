using System;
using TMPro;
using UnityEngine;

public class EnemiesRemainingScript : MonoBehaviour
{
    [SerializeField] protected TextMeshProUGUI rangedEnemyCountText;
    [SerializeField] protected TextMeshProUGUI meleeEnemyCountText;
    [SerializeField] protected GameObject textParent;
 
 
    // Update is called once per frame

    protected bool clearedEnemies;

    void Start() {
        clearedEnemies = false;
        textParent.SetActive(true);
    }

    protected float waitFor = 0.3f;

    protected float currTime = 0f;

    protected bool setIsSticko = false;
    void Update()
    {
        Tuple<int, int> enemiesCount = EnemyManagerScript.Instance.getEnemyCounts();
        meleeEnemyCountText.text = "x" + enemiesCount.Item1.ToString();
        rangedEnemyCountText.text = "x" + enemiesCount.Item2.ToString();
        if (enemiesCount.Item1 == 0 && enemiesCount.Item2 == 0 && !clearedEnemies) {
            if (!setIsSticko) {
                setIsSticko = true;
                PlayerInputManager.Instance.setIsStickoMode(true);
            }
            currTime+=Time.deltaTime;
            if (currTime > waitFor) {
                clearedEnemies = true;
                allEnemiesDead();
            }
        }
    }

    public void allEnemiesDead() {
        GameStateManager.Instance.captureGameState();
        textParent.SetActive(false);
        BeatmapGenerator.Instance.GenerateBeatmap();
        Beatmap beatmap = BeatmapGenerator.Instance.GetBeatmap();
        SongTransitionerController.Instance.startTransition(beatmap);
    }


}
