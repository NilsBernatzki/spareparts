using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct InputVRStruct {
    public ViveHand hand;
    public KeyCodeVR button;
}
public class InputsVR : MonoBehaviour {
    public static InputsVR singleton;
    public InputVRStruct _split;

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
