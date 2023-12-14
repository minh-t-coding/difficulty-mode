using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class BeatmapHit : MonoBehaviour
{
    [SerializeField] protected SpriteRenderer spr;

    [SerializeField] protected GameObject arrow;
    [SerializeField] protected GameObject hitIcon;

    protected Color baseColor;

    protected bool beenHit;

    protected bool beenMissed;

 

    void Start()
    {
        baseColor = spr.color;
    }

    public void setKey(string s) {
        if (s.Equals("W")) {
            arrow.transform.eulerAngles = new Vector3(0,0,0);
        } else if (s.Equals("A")) {
            arrow.transform.eulerAngles = new Vector3(0,0,90);
        } else if (s.Equals("S")) {
            arrow.transform.eulerAngles = new Vector3(0,0,180);
        } else if (s.Equals("D")) {
            arrow.transform.eulerAngles = new Vector3(0,0,270);
        } else {
            arrow.SetActive(false);
            hitIcon.SetActive(true);
        }
        
    }

    protected float distFromBar() {
        return Mathf.Abs(BeatmapBar.Instance.transform.position.y - transform.position.y);
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
                arrow.SetActive(false);
                hitIcon.SetActive(false);
                spr.enabled = false;
            }
        }
    }

    public void CreateHitEffect() {
        beenHit = true;
        Transform status = Instantiate((Resources.Load("Prefabs/BeatmapComponents/hitEffect") as GameObject).transform);
        status.transform.position = new Vector3(transform.position.x,transform.position.y,0);
        HitEffect de = status.GetComponent<HitEffect>();
        de.Setup(baseColor,1f);
        
    }

    public void CreateMissEffect() {
        beenMissed = true;
    }
}
