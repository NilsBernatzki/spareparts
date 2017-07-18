using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MovePart))]
public class Split : MonoBehaviour {

    private WaitForEndOfFrame waitFrame;
    private ChainObject chainObj;
    private float t;
    private float time;
    private float length;
    private MovePart movePart;

    public Vector3 goalPos;
    public bool currentlySplitting;

    // Use this for initialization
    void Start () {
        movePart = GetComponent<MovePart>();
    }
	// Update is called once per frame
	void Update () {
        
    }
    public void SetFields() {
        chainObj = GetComponent<ChainObject>();
        waitFrame = new WaitForEndOfFrame();
    }
    public void SplitUp(float speed) {
        currentlySplitting = true;
        if (GameManager.singleton.finishedSetup) {
            goalPos = chainObj.endPos;
            movePart.Move();
        } else {
            ResetValues();
            StartCoroutine(LerpFromTo(chainObj.startPos, chainObj.endPos, speed));
        }
    }
        
    public void Revert(float speed) {
        currentlySplitting = true;
        if (GameManager.singleton.finishedSetup) {
            goalPos = chainObj.startPos;
            movePart.Move();
        } else {
            ResetValues();
            if (!chainObj.ghost) {
                chainObj.meshObject.GetComponent<PlaceGhost>().active = true;
            }
            StartCoroutine(LerpFromTo(chainObj.endPos, chainObj.startPos, speed));
        }
    }
    private IEnumerator LerpFromTo(Vector3 from, Vector3 to,float speed) {
        while (t < 1f) {
            time += Time.deltaTime;
            t = (time * speed) / length;
            if (t > 1f) {
                t = 1f;
            }
            SetPosition(Vector3.Lerp(from, to, t));
            yield return waitFrame;
        }
        currentlySplitting = false;
    }
    private void SetPosition(Vector3 newPos) {
        transform.position = newPos;
    }
    private void ResetValues() {
        t = 0;
        time = 0;
        length = Vector3.Distance(chainObj.startPos, chainObj.endPos);
    }
    
}
