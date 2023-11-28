using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PumpkinScript : MonoBehaviour
{
    [SerializeField] protected float pumpkinSpeed;
    [SerializeField] protected SpriteRenderer pumpkinSprite;

    void Update()
    {
        Vector3 pathToPlayer = PlayerScript.Instance.transform.position - transform.position;
        
        if (pathToPlayer.x < 0) {
            pumpkinSprite.flipX = true;
        } else {
            pumpkinSprite.flipX = false;
        }

        transform.Translate(pumpkinSpeed * Time.deltaTime * pathToPlayer/pathToPlayer.magnitude);
    }
}
