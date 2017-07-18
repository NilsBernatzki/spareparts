using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class SplittManager : MonoBehaviour {

    public static SplittManager singleton;

    private List<Splittable> _allSplittablesList = new List<Splittable>();

    private GetInputVR vrInput;
    private InputsVR inputs;

    //parameter
    [SerializeField]
    private bool blockInputs;
    public float globalSplitSpeed;

    private void Awake() {
        singleton = this;
    }
    private void Start() {
        vrInput = GetInputVR.singleton;
        inputs = InputsVR.singleton;
    }
    private void Update() {
        if (!vrInput.vrControllersReady) return;
        
        if (!blockInputs) {
            
            if (vrInput.GetKeyDown(inputs._split.button, inputs._split.hand)){
                StartCoroutine(SplitAllSplittables());
                return;
            }
            if (vrInput.GetKeyDown(inputs._revert.button, inputs._revert.hand)) {
                StartCoroutine(RevertAllSplittables());
                return;
            }
        }
    }

    private IEnumerator SplitAllSplittables() {
        blockInputs = true;
        foreach(Splittable splittable in _allSplittablesList) {
            splittable.Split();
            yield return null;
        }
        blockInputs = false;
    }
    private IEnumerator RevertAllSplittables() {
        blockInputs = true;
        foreach (Splittable splittable in _allSplittablesList) {
            splittable.Revert();
            yield return null;
        }
        blockInputs = false;
    }
    //Other
    #region
    public void AddSplittable(Splittable splittable) {
        _allSplittablesList.Add(splittable);
    }
#endregion
}
