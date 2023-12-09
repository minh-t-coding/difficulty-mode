using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitEffect : MonoBehaviour
{
    [SerializeField] private ParticleSystem ps;
    public void Setup(Color color, float time) {
        var main = ps.main;
        main.duration = time;
        if (color != null) {
            main.startColor = color;
        }
        ps.Play();
    }

    public static void CreateHitEffectStatic(Vector3 pos, Color color) {
        Transform status = Instantiate(Resources.Load("Prefabs/BeatmapComponents/hitEffect") as GameObject).transform;
        status.position = pos;
        HitEffect de = status.GetComponent<HitEffect>();
        de.Setup(color,1f);
    }
}
