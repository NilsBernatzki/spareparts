using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Split : MonoBehaviour {

    public bool currentlySplitting;

    private WaitForEndOfFrame waitFrame;
    public ChainObject chainObj;
    private Rigidbody rig;

    private Vector3 goalPos;
    private float splitSpeed;

    private float t;
    private float time;
    private float length;

    // Use this for initialization
    void Start () {
        
    }
	// Update is called once per frame
	void Update () {
        if (GameManager.singleton.finishedSetup) {
            Move();
        }
	}
    public void SetFields() {
        chainObj = GetComponent<ChainObject>();
        waitFrame = new WaitForEndOfFrame();
        rig = GetComponent<Rigidbody>();
        splitSpeed = SplitManager.singleton.speed;
    }
    public void SplitUp(float speed) {
        currentlySplitting = true;
        if (GameManager.singleton.finishedSetup) {
            goalPos = chainObj.endPos;
        } else {
            ResetValues();
            StartCoroutine(LerpFromTo(chainObj.startPos, chainObj.endPos, speed));
        }
    }
        
    public void Revert(float speed) {
        currentlySplitting = true;
        if (GameManager.singleton.finishedSetup) {
            goalPos = chainObj.startPos;
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
    private void Move() {
        if (goalPos == Vector3.zero) return;
        splitSpeed = SplitManager.singleton.speed;
        Vector3 dir = goalPos - transform.position;
        rig.velocity = dir * splitSpeed;
        if (currentlySplitting) {
            float dist = Vector3.Distance(transform.position, goalPos);
            if (dist <= 0.1f) {
                currentlySplitting = false;
            }
        }
    }
}
