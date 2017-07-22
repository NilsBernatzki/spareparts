using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class SplittManager : MonoBehaviour {

    public static SplittManager singleton;
    private GetInputVR vrInput;
    private InputsVR inputs;

    public List<Splittable> _allSplittablesList = new List<Splittable>();
    public GameObject root;
   

    //parameter
    [SerializeField]
    private bool blockInputs;
    private int currentSplitIndex = 0;

    public float globalSplitSpeed;

    private void Awake() {
        singleton = this;
    }
    private void Start() {
        vrInput = GetInputVR.singleton;
        inputs = InputsVR.singleton;
        SortSplittableListWithIndex();
    }
    private void Update() {

        if (inputs.VRMode) {
            if (!vrInput.vrControllersReady) return;
            if (!blockInputs) {
                if (vrInput.GetKeyDown(inputs._split.button, inputs._split.hand)) {
                    StartCoroutine(SplitAllSplittables());
                    return;
                }
                if (vrInput.GetKeyDown(inputs._revert.button, inputs._revert.hand)) {
                    StartCoroutine(RevertAllSplittables());
                    return;
                }
            }
        } else {
            if (!blockInputs) {
                if (Input.GetKeyDown(inputs._splitKey.key)) {
                    StartCoroutine(SplitAllSplittables());
                    currentSplitIndex = _allSplittablesList.Count;
                    return;
                }
                if (Input.GetKeyDown(inputs._revertKey.key)) {
                    StartCoroutine(RevertAllSplittables());
                    currentSplitIndex = 0;
                    return;
                }
                if (Input.GetKeyDown(inputs._splitOneKey.key)) {
                    SplitOnePart(ref currentSplitIndex);
                    return;
                }
                if (Input.GetKeyDown(inputs._revertOneKey.key)) {
                    RevertOnePart(ref currentSplitIndex);
                    return;
                }
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
    private void SplitOnePart(ref int index) {
        if (index == _allSplittablesList.Count) return;
        _allSplittablesList[index].Split();
        index++;
    }
    private void RevertOnePart(ref int index) {
        if (index == 0) return;
        index--;
        _allSplittablesList[index].Revert();
        
    }
    //Other
    #region
    public void AddSplittable(Splittable splittable) {
        _allSplittablesList.Add(splittable);
    }
    private void SortSplittableListWithIndex() {
        _allSplittablesList.Sort((splittableX, splittableY) => splittableX.splitIndex.CompareTo(splittableY.splitIndex));
    }
#endregion
}
