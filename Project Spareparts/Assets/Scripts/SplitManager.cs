using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SplitManager : MonoBehaviour {

    public static SplitManager singleton;
    public float speedSplitOnStart;
    public float speedRevertOnStart;
    public float speed;
    public float oldSpeed;
    public float maxSpeed;
    public float minSpeed;
    public float speedAddSubAmount;
    public bool paused;
    public bool startSplitting;
    public bool startReverting;
    public bool finishedStartSplit;
    private bool reverting;
    private bool splitting;
    public bool splitted;
    
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
        oldSpeed = speed;
        finishedStartSplit = true;
    }
	// Update is called once per frame
	void Update () {
        if (ObjectManager.singleton.model == null) return;
        if (Input.GetKeyDown(KeyCode.LeftControl) && finishedStartSplit) {
            if(!splitting && !reverting) {
                StartCoroutine(Split());
            }
        }
        if (Input.GetKeyDown(KeyCode.LeftAlt) && finishedStartSplit) {
            if (!splitting && !reverting) {
                StartCoroutine(Revert());
            }
        }
        if (!finishedStartSplit) return;
        if (Input.GetKeyDown(KeyCode.P)) {
            paused = !paused;
            StartCoroutine(LerpSpeed());
        }
        if (Input.GetKeyDown(KeyCode.UpArrow)) {
            oldSpeed += speedAddSubAmount;
            if (oldSpeed > maxSpeed) {
                oldSpeed = maxSpeed;
            }
            if (paused) return;
            speed += speedAddSubAmount;
            if(speed > maxSpeed) {
                speed = maxSpeed;
            }
        }
        if (Input.GetKeyDown(KeyCode.DownArrow)) {
            oldSpeed -= speedAddSubAmount;
            if (oldSpeed < minSpeed) {
                oldSpeed = minSpeed;
            }
            if (paused) return;
            speed -= speedAddSubAmount;
            if (speed < minSpeed) {
                speed = minSpeed;
            }
        }
    }
    private IEnumerator LerpSpeed() {
        float t = 0;
        if (paused) {
            while(t <= 1) {
                t += Time.deltaTime * 2f;
                speed = Mathf.Lerp(speed, 0f, t);
                yield return new WaitForEndOfFrame();
            }
        } else {
            while (t <= 1) {
                t += Time.deltaTime * 2f;
                speed = Mathf.Lerp(speed, oldSpeed, t);
                yield return new WaitForEndOfFrame();
            }
        }
    }
    private IEnumerator Split() {
        splitting = true;
        for (int i = ObjectManager.singleton.socketList.Count - 1; i >= 0; i--) {
            GameObject obj = ObjectManager.singleton.socketList[i].attachedAttachPoint.transform.parent.gameObject;
            obj.GetComponent<Split>().SplitUp(speed);
            yield return new WaitUntil(() => !obj.GetComponent<Split>().currentlySplitting);
        }
        splitting = false;
        splitted = true;
    }
    private IEnumerator Revert() {
        reverting = true;
        for (int i = 0; i < ObjectManager.singleton.socketList.Count; i++) {
            GameObject obj = ObjectManager.singleton.socketList[i].attachedAttachPoint.transform.parent.gameObject;
            obj.GetComponent<ChainObject>().meshObject.GetComponent<MeshRenderer>().enabled = true;
            obj.GetComponent<Split>().Revert(speed);
            yield return new WaitUntil(() => !obj.GetComponent<Split>().currentlySplitting);
        }
        reverting = false;
        splitted = false;
    }
    private IEnumerator SplitOnStart() {
        startSplitting = true;
        for (int i = ObjectManager.singleton.socketList.Count -1; i >= 0; i--) {
            GameObject obj = ObjectManager.singleton.socketList[i].attachedAttachPoint.transform.parent.gameObject;
            obj.GetComponent<Split>().SplitUp(speedSplitOnStart);
        }
        yield return new WaitForSeconds(1f);
        startSplitting = false;
    }
    private IEnumerator RevertOnStart() {
        startReverting = true;
        for (int i = 0; i < ObjectManager.singleton.socketList.Count; i++) {
            GameObject obj = ObjectManager.singleton.socketList[i].attachedAttachPoint.transform.parent.gameObject;
            obj.GetComponent<ChainObject>().meshObject.GetComponent<MeshRenderer>().enabled = true;
            obj.GetComponent<Split>().Revert(speedRevertOnStart);
            yield return new WaitUntil(() => !obj.GetComponent<Split>().currentlySplitting);
        }
        startReverting = false;
    }
}
