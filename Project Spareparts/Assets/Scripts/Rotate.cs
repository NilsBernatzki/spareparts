using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotate : MonoBehaviour {

    private float rotationSpeed = 1f;
    public bool rotationMode;
    private ChainObject myChainObject;
    private SelectObject select;
	// Use this for initialization
	void Start () {
        myChainObject = GetComponent<ChainObject>();
        select = GameManager.singleton.GetComponent<SelectObject>();
        select.enterSingleViewChainObjectEvent += SwitchToRotationMode;
        select.exitSingleViewChainObjectEvent += ExitRotationMode;
	}
	
	// Update is called once per frame
	void Update () {
        if (rotationMode) {
            MouseDrag();
        }
    }
    private void ExitRotationMode(ChainObject chainObject) {
        if (chainObject == myChainObject) {
            rotationMode = false;
            Debug.Log("RotationMode OFF");
        }
    }
    private void SwitchToRotationMode(ChainObject chainObject) {
        Debug.Log(chainObject.name);
        if (chainObject == myChainObject) {
            rotationMode = true;
            Debug.Log("RotationMode ON");
        }
    }
    private void MouseDrag() {
       if(Input.GetMouseButton(0)) {
            Debug.Log("RotationMode ROTATIG");
            float rotX = Input.GetAxis("Mouse X") * rotationSpeed * Mathf.Rad2Deg;
            float rotY = Input.GetAxis("Mouse Y") * rotationSpeed * Mathf.Rad2Deg;
            transform.Rotate(Vector3.up, -rotX);
            transform.Rotate(Vector3.right, rotY);
        }
    }
}
