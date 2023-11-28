using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PumpkinScript : MonoBehaviour
{
    public Transform player;
    public float pumpkinSpeed;
    public SpriteRenderer pumpkinSprite;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        var path = player.transform.position - transform.position;
        
        if (path.x < 0) {
            pumpkinSprite.flipX = true;
        } else {
            pumpkinSprite.flipX = false;
        }

        transform.Translate(pumpkinSpeed * Time.deltaTime * path/path.magnitude);
    }
}
