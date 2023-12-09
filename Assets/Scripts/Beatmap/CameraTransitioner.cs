using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MEC;
public class CameraTransitioner : MonoBehaviour
{
    [SerializeField] protected Transform beatmapTex;

    [SerializeField] protected Transform mainTex;

    [SerializeField] protected Transform mainCam;

    [SerializeField] protected GameObject beatmap;

    protected bool hasStarted;
    void init()
    {
        hasStarted=true;

    }

    void Update() {
        if (!hasStarted && Input.GetKey(KeyCode.Return)) {
            init();
            Timing.RunCoroutine(transitionCo());
        }
    }

    protected IEnumerator<float> transitionCo() {
        //Timing.RunCoroutine(transOverSecondsCo(mainTex.gameObject,new Vector3(mainTex.localScale.x,mainTex.localScale.y,mainTex.localScale.z) *0.8f,2f));
        //yield return Timing.WaitUntilDone(MoveOverSecondsCoroutine(mainTex.gameObject,new Vector3(0,0,0),2f).CancelWith(gameObject),this.gameObject.GetInstanceID());
        yield return Timing.WaitForSeconds(1f);
        beatmap.SetActive(true);
    }

    protected IEnumerator<float> MoveOverSecondsCoroutine (GameObject objectToMove, Vector3 distance, float seconds) {
        float elapsedTime = 0;
        Vector3 startingPos = objectToMove.transform.position;
        Vector3 endPos = startingPos + distance;
        while (elapsedTime <seconds) {
            objectToMove.transform.position = Vector3.Lerp(startingPos, endPos, (elapsedTime / seconds));
            elapsedTime += Time.deltaTime;
            yield return Timing.WaitForOneFrame;
        }
        objectToMove.transform.position = endPos;
    }

    public IEnumerator<float> transOverSecondsCo (GameObject objectToMove, Vector3 scale, float seconds) {
        float elapsedTime = 0;
        Vector3 startingPos = objectToMove.transform.localScale;
        Vector3 endPos = new Vector3(scale.x,scale.y,scale.z);
        while (elapsedTime < seconds) {
            objectToMove.transform.localScale = Vector3.Lerp(startingPos, endPos, (elapsedTime / seconds));
            elapsedTime += Time.deltaTime;
            yield return Timing.WaitForOneFrame;
        }
        objectToMove.transform.localScale = endPos;
    }
}
