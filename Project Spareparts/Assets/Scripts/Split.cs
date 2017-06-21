using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Split : MonoBehaviour {

    private WaitForEndOfFrame waitFrame;
    private ChainObject chainObj;

    private Vector3 dir;
    private Vector3 goalPos;
    private Vector3 startPos;

    private float t;
    private float time;
    private float length;

    // Use this for initialization
    void Start () {
        waitFrame = new WaitForEndOfFrame();
        chainObj = GetComponent<ChainObject>();
        dir = chainObj.splitVector;
    }
	
	// Update is called once per frame
	void Update () {
		
	}
    public void SplitUp(float speed) {
        startPos = transform.position;
        goalPos = chainObj.endPos;
        if (goalPos == Vector3.zero) {
            goalPos = transform.position + dir * 10;
        }
        ResetValues();
        StartCoroutine(LerpFromTo(startPos, goalPos, speed));
    }
    public void Revert(float speed) {

        if(chainObj.endPos == Vector3.zero) {
            chainObj.meshObject.GetComponent<PlaceGhost>().active = true;
        }

        goalPos = chainObj.startPos;
        startPos = transform.position;

        ResetValues();
        StartCoroutine(LerpFromTo(startPos, goalPos, speed));
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
        SplitManager.singleton.splitting = false;
    }
    private void SetPosition(Vector3 newPos) {
        transform.position = newPos;
    }
    private void ResetValues() {
        t = 0;
        time = 0;
        length = Vector3.Distance(startPos, goalPos);
    }
}
