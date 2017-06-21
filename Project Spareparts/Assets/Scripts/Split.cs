using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Split : MonoBehaviour {

    public bool currentlySplitting;

    private WaitForEndOfFrame waitFrame;
    public ChainObject chainObj;

    public Vector3 dir;
    private Vector3 goalPos;
    private Vector3 startPos;

    private float t;
    private float time;
    private float length;

    // Use this for initialization
    void Start () {
        
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
        ResetValues();
        StartCoroutine(LerpFromTo(chainObj.startPos, chainObj.endPos, speed));
    }
    public void Revert(float speed) {
        currentlySplitting = true;
        ResetValues();
        if (!chainObj.ghost) {
            chainObj.meshObject.GetComponent<PlaceGhost>().active = true;
        }
        StartCoroutine(LerpFromTo(chainObj.endPos, chainObj.startPos, speed));
    }

   /*public void SplitUp(float speed) {
        currentlySplitting = true;
        if(chainObj.startPos == Vector3.zero) {
            chainObj.startPos = transform.position;
        }
        startPos = transform.position;
        goalPos = chainObj.endPos;
        if (goalPos == Vector3.zero) {
            goalPos = transform.position + chainObj.splitVector * 10;
        }
        ResetValues();
        StartCoroutine(LerpFromTo(startPos, goalPos, speed));
    }
    public void Revert(float speed) {
        currentlySplitting = true;
        if (chainObj.endPos == Vector3.zero) {
            chainObj.meshObject.GetComponent<PlaceGhost>().active = true;
        }
        goalPos = chainObj.startPos;
        startPos = transform.position;
        ResetValues();
        StartCoroutine(LerpFromTo(startPos, goalPos, speed));
    }
    */
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
