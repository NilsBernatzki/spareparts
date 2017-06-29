using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum SplitBehavior {SplitSmoothVelocity};

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

    private SelectObject selectObject;
    public SplitBehavior splitBehavior;

    private void Awake() {
        singleton = this;
    }

    // Use this for initialization
    void Start () {
        selectObject = GameManager.singleton.GetComponent<SelectObject>();
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
                StartCoroutine(FullSplit());
            }
        }
        if (Input.GetKeyDown(KeyCode.LeftAlt) && finishedStartSplit) {
            if (!splitting && !reverting) {
                StartCoroutine(FullRevert());
            }
        }
        if (!finishedStartSplit) return;
        if (Input.GetKeyDown(KeyCode.P)) {
            paused = !paused;
            StartCoroutine(LerpSpeedOnPause());
        }
        if (Input.GetKeyDown(KeyCode.KeypadPlus)) {
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
        if (Input.GetKeyDown(KeyCode.KeypadMinus)) {
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
    private IEnumerator LerpSpeedOnPause() {
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
    private IEnumerator FullSplit() {
        splitting = true;
        for (int i = ObjectManager.singleton.socketList.Count - 1; i >= 0; i--) {
            GameObject obj = ObjectManager.singleton.socketList[i].attachedAttachPoint.transform.parent.gameObject;
            obj.GetComponent<Split>().SplitUp(speed);
            if(i == 0 || selectObject.selectionMode) {
                yield return new WaitUntil(() => !obj.GetComponent<Split>().currentlySplitting);
            }
            yield return new WaitForEndOfFrame();
        }
        yield return new WaitForSeconds(1.5f);
        splitting = false;
        splitted = true;
    }
    private IEnumerator FullRevert() {
        reverting = true;
        for (int i = 0; i < ObjectManager.singleton.socketList.Count; i++) {
            GameObject obj = ObjectManager.singleton.socketList[i].attachedAttachPoint.transform.parent.gameObject;
            obj.GetComponent<ChainObject>().meshObject.GetComponent<MeshRenderer>().enabled = true;
            obj.GetComponent<Split>().Revert(speed);
            if (i == ObjectManager.singleton.socketList.Count-1 || selectObject.selectionMode) {
                yield return new WaitUntil(() => !obj.GetComponent<Split>().currentlySplitting);
            }
            yield return new WaitForEndOfFrame();
        }
        yield return new WaitForSeconds(1.5f);
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
