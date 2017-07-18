using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SplittManager : MonoBehaviour {

    public static SplittManager singleton;

    private List<Splittable> _allSplittablesList = new List<Splittable>();

    private GetInputVR vrInput;

    //parameter
    private bool blockInputs;

    private void Awake() {
        singleton = this;
    }
    private void Start() {
        vrInput = GetInputVR.singleton;
    }
    private void Update() {

        if (!blockInputs) {

            if (vrInput.GetKeyDown(KeyCodeVR.trigger, ViveHand.left)){
                Debug.Log("test");
                //StartCoroutine(SplitAllSplittables());
            }

        }

    }

    private IEnumerator SplitAllSplittables() {
        blockInputs = true;
        foreach(Splittable splittable in _allSplittablesList) {
            splittable.Split();
            yield return null;
        }
        blockInputs = true;
    }

    //Other
#region
    public void AddSplittable(Splittable splittable) {
        _allSplittablesList.Add(splittable);
    }
#endregion
}
