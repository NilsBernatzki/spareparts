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

        inputs.SplitOne += () => { SplitOnePart(); };
        inputs.RevertOne += () => { RevertOnePart(); };
        inputs.SplitAll += () => { StartCoroutine(SplitAllSplittables()); };
        inputs.RevertAll += () => { StartCoroutine(RevertAllSplittables()); };

    }
    private void Update() {

        if (inputs.VRMode) {
            if (!vrInput.vrControllersReady) return;
            
        } else {
            ReactOnKeyboardInputs();
        }
    }
    private void ReactOnKeyboardInputs() {
        if (!blockInputs) {
            if (Input.GetKeyDown(inputs._splitKey.key)) {
                StartCoroutine(SplitAllSplittables());
                return;
            }
            if (Input.GetKeyDown(inputs._revertKey.key)) {
                StartCoroutine(RevertAllSplittables());
                return;
            }
            if (Input.GetKeyDown(inputs._splitOneKey.key)) {
                SplitOnePart();
                return;
            }
            if (Input.GetKeyDown(inputs._revertOneKey.key)) {
                RevertOnePart();
                return;
            }
        }
    }
    private IEnumerator SplitAllSplittables() {
        if (!blockInputs) {
            blockInputs = true;
            foreach (Splittable splittable in _allSplittablesList) {
                //splittable.Split();
                SplitOnePart();
                yield return null;
            }
            blockInputs = false;
        }
    }
    private IEnumerator RevertAllSplittables() {
        if (!blockInputs) {
            blockInputs = true;
            foreach (Splittable splittable in _allSplittablesList) {
                //splittable.Revert();
                RevertOnePart();
                yield return null;
            }
            blockInputs = false;
        }
    }
    private void SplitOnePart() {
        if (currentSplitIndex == _allSplittablesList.Count) return;
        _allSplittablesList[currentSplitIndex].Split();
        currentSplitIndex++;
    }
    private void RevertOnePart() {
        if (currentSplitIndex == 0) return;
        currentSplitIndex--;
        _allSplittablesList[currentSplitIndex].Revert();
        
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
