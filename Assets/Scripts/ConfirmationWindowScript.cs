using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConfirmationWindowScript : MonoBehaviour
{
    public void OnButtonClick() {
        PlayerPrefs.DeleteAll();
    }
}
