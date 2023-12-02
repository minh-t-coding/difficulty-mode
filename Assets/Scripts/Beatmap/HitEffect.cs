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
}
