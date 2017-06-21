using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SplitManager : MonoBehaviour {

    public static SplitManager singleton;
    public float speedOnStart;
    public bool splitting;
    private WaitForEndOfFrame waitAFrame;
    private void Awake() {
        singleton = this;
    }

    // Use this for initialization
    void Start () {
        waitAFrame = new WaitForEndOfFrame();
    }
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown(KeyCode.LeftControl) && !splitting) {
            StartCoroutine(SplitOnStart());
        }
        if (Input.GetKeyDown(KeyCode.LeftAlt) && !splitting) {
            StartCoroutine(RevertOnStart());
        }
	}
    private IEnumerator SplitOnStart() {
        for(int i = ObjectManager.singleton.socketList.Count -1; i >= 0; i--) {
            splitting = true;
            GameObject obj = ObjectManager.singleton.socketList[i].attachedAttachPoint.transform.parent.gameObject;
            obj.GetComponent<Split>().SplitUp(speedOnStart);
            yield return new WaitUntil(() => !obj.GetComponent<Split>().currentlySplitting);
        }
        splitting = false;
    }
    private IEnumerator RevertOnStart() {
        for (int i = 0; i < ObjectManager.singleton.socketList.Count; i++) {
            splitting = true;
            GameObject obj = ObjectManager.singleton.socketList[i].attachedAttachPoint.transform.parent.gameObject;
            obj.GetComponent<ChainObject>().meshObject.GetComponent<MeshRenderer>().enabled = true;
            obj.GetComponent<Split>().Revert(speedOnStart);
            yield return new WaitUntil(() => !obj.GetComponent<Split>().currentlySplitting);
        }
        splitting = false;
    }
}
