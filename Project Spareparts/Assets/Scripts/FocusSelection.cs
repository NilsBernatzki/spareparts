using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FocusSelection : MonoBehaviour {

    private SelectObject selectObject;
    public ChainObject selectedChainObject;

    private Quaternion startRot;
    private Vector3 startPos;

    private bool focused;
    public float animationSpeed;

	// Use this for initialization
	void Start () {
        startRot = transform.rotation;
        startPos = transform.position;

        selectObject = GameManager.singleton.GetComponent<SelectObject>();
        selectObject.selectedChainObjectEvent += FocusSelectedChainObject;
        selectObject.deselectChainObjectEvent += ResetTransform;
    }
	
	// Update is called once per frame
	void Update () {

	}
    
    private void FocusSelectedChainObject(ChainObject chainObject) {
        print("focus");
        StartCoroutine(Rotate(chainObject));
        StartCoroutine(CloseUp(chainObject));
    }
    private IEnumerator Rotate(ChainObject chainObject) {
        Vector3 dir;
        Quaternion tempStartRot = transform.rotation;
        float t = 0;
        while (t < 1) {
            dir = chainObject.transform.position - transform.position;
            t += Time.fixedDeltaTime / 10 * animationSpeed;
            transform.rotation = Quaternion.Slerp(tempStartRot, Quaternion.LookRotation(dir), Mathf.Pow(t, 2));
            yield return new WaitForEndOfFrame();
        }
    }
    private IEnumerator CloseUp(ChainObject chainObject) {
        Vector3 dir = chainObject.transform.position - transform.position;
        Vector3 goalPos = transform.position + dir / 2f;
        Vector3 tempStartPos = transform.position;
        float t = 0;
        while (t < 1) {
            t += Time.fixedDeltaTime / 10 * animationSpeed;
            transform.position = Vector3.Slerp(tempStartPos, goalPos, Mathf.Pow(t, 2));
            yield return new WaitForEndOfFrame();
        }
    }
    private void ResetTransform(ChainObject chainObject) {
        print("defocus");
        StartCoroutine(ResetPosition());
        StartCoroutine(ResetRotation());
    }
    private IEnumerator ResetRotation() {
        float t = 0;
        Quaternion tempStartRot = transform.rotation;
        while (t < 1) {
            t += Time.fixedDeltaTime / 10 * animationSpeed;
            transform.rotation = Quaternion.Slerp(tempStartRot, startRot, Mathf.Pow(t,2));
            yield return new WaitForEndOfFrame();
        }
    }
    private IEnumerator ResetPosition() {
        float t = 0;
        Vector3 tempStartPos = transform.position;
        while (t < 1) {
            t += Time.fixedDeltaTime / 10 * animationSpeed;
            transform.position = Vector3.Slerp(tempStartPos, startPos, Mathf.Pow(t, 2));
            yield return new WaitForEndOfFrame();
        }
    }
}
