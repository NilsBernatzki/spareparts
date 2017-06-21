using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SplitManager : MonoBehaviour {

    public static SplitManager singleton;
    public float speedOnStart;
    public float speed;
    public bool startSplitting;
    public bool startReverting;
    public bool finishedStartSplit;
    private void Awake() {
        singleton = this;
    }

    // Use this for initialization
    void Start () {

    }
	public void SetUp() {
        StartCoroutine(StartSetupCoroutine());
    }
    private IEnumerator StartSetupCoroutine() {
        StartCoroutine(SplitOnStart());
        yield return new WaitUntil(() => !startSplitting);
        StartCoroutine(RevertOnStart());
        yield return new WaitUntil(() => !startReverting);
        finishedStartSplit = true;
    }
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown(KeyCode.LeftControl) && !startSplitting) {
            StartCoroutine(SplitOnStart());
        }
        if (Input.GetKeyDown(KeyCode.LeftAlt) && !startSplitting) {
            StartCoroutine(RevertOnStart());
        }
	}
    private IEnumerator SplitOnStart() {
        startSplitting = true;
        for (int i = ObjectManager.singleton.socketList.Count -1; i >= 0; i--) {
            GameObject obj = ObjectManager.singleton.socketList[i].attachedAttachPoint.transform.parent.gameObject;
            obj.GetComponent<Split>().SplitUp(speedOnStart);
            yield return new WaitUntil(() => !obj.GetComponent<Split>().currentlySplitting);
        }
        startSplitting = false;
    }
    private IEnumerator RevertOnStart() {
        startReverting = true;
        for (int i = 0; i < ObjectManager.singleton.socketList.Count; i++) {
            GameObject obj = ObjectManager.singleton.socketList[i].attachedAttachPoint.transform.parent.gameObject;
            obj.GetComponent<ChainObject>().meshObject.GetComponent<MeshRenderer>().enabled = true;
            obj.GetComponent<Split>().Revert(speedOnStart);
            yield return new WaitUntil(() => !obj.GetComponent<Split>().currentlySplitting);
        }
        startReverting = false;
    }
}
