using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SingleView : MonoBehaviour {

    private SelectObject select;
    private FocusSelection focus;
    private Vector3 tempFocusPos;
    private Quaternion tempFocusRot;
    public ChainObject singleViewChainObject;
    public GameObject pivot;

	// Use this for initialization
	void Start () {
        select = GameManager.singleton.GetComponent<SelectObject>();
        focus = GetComponent<FocusSelection>();
        select.enterSingleViewChainObjectEvent += EnterSingleView;
        select.exitSingleViewChainObjectEvent += ExitSingleView;
	}
	
	// Update is called once per frame
	void Update () {
		
	}
    private void EnterSingleView(ChainObject chainObject) {
        singleViewChainObject = chainObject;
        SetTempValues();
        StartCoroutine(SetTransparency(0));
        StartCoroutine(CloseUp(chainObject));
        StartCoroutine(Rotate(chainObject));
        SetTempPivotToChainObject(chainObject);
    }
    private void ExitSingleView(ChainObject chainObject) {
        StartCoroutine(SetTransparency(focus.alpha));
        StartCoroutine(ResetPosition(chainObject));
        StartCoroutine(ResetRotation(chainObject));
    }
    private void SetTempValues() {
        tempFocusPos = transform.position;
        tempFocusRot = transform.rotation;
    }
private IEnumerator SetTransparency(float alpha) {
        foreach (GameObject mesh in ObjectManager.singleton.meshes) {
            if (mesh.GetComponent<MeshObject>().chainObject == singleViewChainObject) continue;
            mesh.GetComponent<ChangeColor>().Transparent(alpha);
            yield return new WaitForEndOfFrame();
        }
    }
    private IEnumerator CloseUp(ChainObject chainObject) {
        GameObject mesh = chainObject.meshObject;
        Vector3 dir = mesh.transform.up;
        Vector3 goalPos = mesh.transform.position + dir * (mesh.GetComponent<CapsuleCollider>().height*100f);
        Vector3 tempStartPos = transform.position;
        float t = 0;
        while (t < 1) {
            t += Time.fixedDeltaTime / 10 * focus.animationSpeed;
            transform.position = Vector3.Slerp(tempStartPos, goalPos, Mathf.Pow(t, 2));
            yield return new WaitForEndOfFrame();
        }
        UnChildFromChainObject();
    }
    private IEnumerator Rotate(ChainObject chainObject) {
        GameObject mesh = chainObject.meshObject;
        Vector3 dir;
        Quaternion tempStartRot = transform.rotation;
        float t = 0;
        while (t < 1) {
            dir = mesh.transform.position - transform.position;
            t += Time.fixedDeltaTime / 10 * focus.animationSpeed;
            transform.rotation = Quaternion.Slerp(tempStartRot, Quaternion.LookRotation(dir), Mathf.Pow(t, 2));
            yield return new WaitForEndOfFrame();
        }
    }
    private IEnumerator ResetPosition(ChainObject chainObject) {
        ParentToChainObject(chainObject);
        float t = 0;
        Vector3 tempStartPos = transform.position;
        while (t < 1) {
            t += Time.fixedDeltaTime / 10 * focus.animationSpeed;
            transform.position = Vector3.Slerp(tempStartPos, tempFocusPos, Mathf.Pow(t, 2));
            yield return new WaitForEndOfFrame();
        }
    }
    private IEnumerator ResetRotation(ChainObject chainObject) {
        float t = 0;
        Quaternion tempStartRot = transform.rotation;
        while (t < 1) {
            t += Time.fixedDeltaTime / 10 * focus.animationSpeed;
            transform.rotation = Quaternion.Slerp(tempStartRot, tempFocusRot, Mathf.Pow(t, 2));
            yield return new WaitForEndOfFrame();
        }
    }
    private void ParentToChainObject(ChainObject chainObject) {
        transform.parent = chainObject.transform;
    }
    private void UnChildFromChainObject() {
        transform.parent = null;
    }
    private void SetTempPivotToChainObject(ChainObject chainObject) {
        Vector3 colliderCenter = chainObject.transform.position; 
        GameObject tempPivot = Instantiate(pivot, colliderCenter, Quaternion.identity);
        chainObject.transform.parent = tempPivot.transform;
    }
}
