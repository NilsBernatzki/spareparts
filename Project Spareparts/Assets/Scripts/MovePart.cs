using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovePart : MonoBehaviour {

    private Split split;
    private Rigidbody rig;
    private float splitSpeed;
    private Vector3 goalPos;

    private SplitBehavior splitBehavior;

    // Use this for initialization
    public void Start() {
        splitBehavior = SplitManager.singleton.splitBehavior;
        split = GetComponent<Split>();
        rig = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update() {
       
    }
    public void Move() {
       UpdateGoalPos();
       switch (splitBehavior) {
            case SplitBehavior.SplitSmoothVelocity:
                StartCoroutine(SmoothVelocityCo());
                break;
        }
    }
    private IEnumerator SmoothVelocityCo() {
        
        if (goalPos != Vector3.zero) {
            while (split.currentlySplitting) {
                SmoothVelocity();
                yield return new WaitForEndOfFrame();
            } 
        }
    }
    private void SmoothVelocity() {
        splitSpeed = SplitManager.singleton.speed;
        Vector3 dir = goalPos - transform.position;
        rig.velocity = dir * splitSpeed;
        if (split.currentlySplitting) {
            float dist = Vector3.Distance(transform.position, goalPos);
            if (dist <= 0.05f) {
                rig.velocity = Vector3.zero;
                StartCoroutine(LerpToGoalPos());
                split.currentlySplitting = false;
            }
        }
    }
    private IEnumerator LerpToGoalPos() {
        float t = 0;
        Vector3 tempPos = rig.position;
        while(t < 1f) {
            t += Time.deltaTime * SplitManager.singleton.speed;
            rig.MovePosition(Vector3.Lerp(tempPos, goalPos, t));
            yield return new WaitForEndOfFrame();
        }
    }
    private void UpdateGoalPos() {
        goalPos = split.goalPos;
    }
}
