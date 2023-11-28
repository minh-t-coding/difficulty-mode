using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraScript : MonoBehaviour
{
    void Update()
    {
        transform.position = PlayerScript.Instance.transform.position + new Vector3(0, 0, -10);
    }
}
