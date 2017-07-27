using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct InputVRStruct {
    public ViveHand hand;
    public KeyCodeVR button;
    public float activationTime;
   // [HideInInspector]
    public float activationTimer;
}
[System.Serializable]
public struct InputKeyStruct {
    public KeyCode key;
}
public class InputsVR : MonoBehaviour {

    public static InputsVR singleton;
    private GetInputVR _vrInput;
    public bool VRMode = false;
    [Header("VR")]

    public InputVRStruct _split;
    public InputVRStruct _revert;
    public InputVRStruct _grab;
    
    public event System.Action SplitOne;
    public event System.Action SplitAll;
    public event System.Action RevertOne;
    public event System.Action RevertAll;
    public event System.Action<WandController> Grab;
    public event System.Action<WandController> Release;

    [Header("Keyboard")]
    public InputKeyStruct _splitKey;
    public InputKeyStruct _revertKey;
    public InputKeyStruct _splitOneKey;
    public InputKeyStruct _revertOneKey;
    public InputKeyStruct _singleView;

    

    private void Awake() {
        singleton = this;
    }
    // Use this for initialization
    void Start() {
        _vrInput = GetInputVR.singleton;
    }

    // Update is called once per frame
    void Update () {
        if (VRMode) {
            UpdateVrInputStructs();
            CheckForGrabAndRelease();
        }
	}
    
    private void CheckForGrabAndRelease() {
        if (_vrInput.GetKeyDown(_grab.button, ViveHand.left)) {
            //Debug.Log("Grab left");
            GrabEvent(ViveHand.left);
        }
        if (_vrInput.GetKeyDown(_grab.button, ViveHand.right)) {
            //Debug.Log("Grab right");
            GrabEvent(ViveHand.right);
        }
        if (_vrInput.GetKeyUP(_grab.button, ViveHand.left)) {
            //Debug.Log("Release left");
            ReleaseEvent(ViveHand.left);
        }
        if (_vrInput.GetKeyUP(_grab.button, ViveHand.right)) {
            //Debug.Log("Release right");
            ReleaseEvent(ViveHand.right);
        }
    }

    private void UpdateVrInputStructs() {
        UpdateTimerSplit();
        UpdateTimerRevert();
    }
    private void UpdateTimerRevert() {
        if (_vrInput.GetKeyUP(_revert.button, _revert.hand)) {
            if (_revert.activationTimer <= _revert.activationTime) {
                RevertOneEvent();
            } else {
                RevertAllEvent();
            }
            _revert.activationTimer = 0;
            return;
        }
        if (_vrInput.GetKey(_revert.button, _revert.hand)) {
            _revert.activationTimer += Time.deltaTime;
            return;
        }
    }
    private void UpdateTimerSplit() {
        if (_vrInput.GetKeyUP(_split.button, _split.hand)) {
            if(_split.activationTimer <= _split.activationTime) {
                SplitOneEvent();
            } else {
                SplitAllEvent();
            }
            _split.activationTimer = 0;
            return;
        }
        if (_vrInput.GetKey(_split.button, _split.hand)) {
            _split.activationTimer += Time.deltaTime;
            return;
        }
    }

    //############## Events ##############
    //Split
    private void SplitOneEvent() {
        if (SplitOne != null) {
            SplitOne();
        }
    }
    private void SplitAllEvent() {
        if (SplitAll != null) {
            SplitAll();
        }
    }
    //Revert
    private void RevertOneEvent() {
        if (RevertOne != null) {
            RevertOne();
        }
    }
    private void RevertAllEvent() {
        if (RevertAll != null) {
            RevertAll();
        }
    }
    //Grab/Release
    private void GrabEvent(ViveHand hand) {
        if(Grab != null) {
            WandController controller = _vrInput.GetWandController(hand);
            Grab(controller);
        }
    }
    private void ReleaseEvent(ViveHand hand) {
        if(Release != null) {
            WandController controller = _vrInput.GetWandController(hand);
            Release(controller);
        }
    }
}
