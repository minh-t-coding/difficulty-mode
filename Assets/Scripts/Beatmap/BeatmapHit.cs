using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class BeatmapHit : MonoBehaviour
{
    [SerializeField] protected SpriteRenderer spr;

    [SerializeField] protected Text textBox;

    protected Color baseColor;

    protected bool beenHit;

    protected bool beenMissed;

 

    void Start()
    {
        baseColor = spr.color;
    }

    public void setKey(string s) {
        textBox.text = s;
    }

    protected float distFromBar() {
        return (BeatmapBar.Instance.transform.position - transform.position).magnitude;
    }

    protected float colorFunc(float dist) {
        float threshold = 4f;

        float maxThreshold = 20f;

        if (dist>maxThreshold) {
            return 0f;
        }
        if (dist<threshold) {
            return 1f;
        }
        return (threshold/(dist+1f)) * (threshold/(dist+1f)) ;

        

    }

    // Update is called once per frame
    void Update()
    {
        if (!beenHit && !beenMissed) {
            spr.color = new Color(baseColor.r,baseColor.g,baseColor.b, colorFunc(distFromBar()) );
        } else {
            if (beenMissed) {
                 spr.color = new Color(0.4f,0.4f,0.4f, colorFunc(distFromBar()) );
            }

            if (beenHit) {
                spr.enabled = false;
            }
        }
    }

    public void CreateHitEffect() {
        beenHit = true;
        Transform status = Instantiate((Resources.Load("Prefabs/BeatmapComponents/hitEffect") as GameObject).transform,transform.position, Quaternion.identity);
        HitEffect de = status.GetComponent<HitEffect>();
        de.Setup(baseColor,1f);
        
    }

    public void CreateMissEffect() {
        beenMissed = true;
    }
}