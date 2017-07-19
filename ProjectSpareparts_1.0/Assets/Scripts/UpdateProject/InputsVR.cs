using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct InputVRStruct {
    public ViveHand hand;
    public KeyCodeVR button;
}
[System.Serializable]
public struct InputKeyStruct {
    public KeyCode key;
}
public class InputsVR : MonoBehaviour {

    public static InputsVR singleton;

    public bool VRMode = false;
    [Header("VR")]
    public InputVRStruct _split;
    public InputVRStruct _revert;

    [Header("Keyboard")]
    public InputKeyStruct _splitKey;
    public InputKeyStruct _revertKey;
    public InputKeyStruct _splitOneKey;
    public InputKeyStruct _revertOneKey;

    private void Awake() {
        singleton = this;
    }
    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
