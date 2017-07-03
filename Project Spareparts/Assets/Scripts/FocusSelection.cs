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
    [Range(0, 1)]
    public float alpha;

    // Use this for initialization
    void Start () {
        startRot = transform.rotation;
        startPos = transform.position;

        selectObject = GameManager.singleton.GetComponent<SelectObject>();
        selectObject.selectedChainObjectEvent += FocusSelectedChainObject;
        selectObject.deselectChainObjectEvent += DefocusSelectedChainObject;
    }
	
	// Update is called once per frame
	void Update () {

	}
    
    private void FocusSelectedChainObject(ChainObject chainObject) {
        StartCoroutine(TransparentAll());
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
        ParentToChainObject(chainObject);
    }
    private void ParentToChainObject(ChainObject chainObject) {
        transform.parent = chainObject.transform;
    }
    private void DefocusSelectedChainObject(ChainObject chainObject) {
        StartCoroutine(UnTransparentAll());
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
        UnChildFromChainObject();
        float t = 0;
        Vector3 tempStartPos = transform.position;
        while (t < 1) {
            t += Time.fixedDeltaTime / 10 * animationSpeed;
            transform.position = Vector3.Slerp(tempStartPos, startPos, Mathf.Pow(t, 2));
            yield return new WaitForEndOfFrame();
        }
    }
    private void UnChildFromChainObject() {
        transform.parent = null;
    }
    private IEnumerator TransparentAll() {
        foreach (GameObject mesh in ObjectManager.singleton.meshes) {
            if (mesh.GetComponent<MeshObject>().chainObject == selectedChainObject) continue;

            mesh.GetComponent<ChangeColor>().Transparent(alpha);
            yield return new WaitForEndOfFrame();
        }
    }
    private IEnumerator UnTransparentAll() {
        foreach (GameObject mesh in ObjectManager.singleton.meshes) {
            if (mesh.GetComponent<MeshObject>().chainObject == selectedChainObject) continue;
            mesh.GetComponent<ChangeColor>().Untransparent();
            yield return new WaitForEndOfFrame();
        }
    }
}
