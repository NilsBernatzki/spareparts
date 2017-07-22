using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateOnDrag : MonoBehaviour {
    private ObjectSelection selection;
    private bool singleView;
    private float rotationSpeed;
    // Use this for initialization
    void Start () {
        selection = InputsVR.singleton.GetComponent<ObjectSelection>();
        selection.enterSingleViewSplittableEvent += EnterSingleView;
        selection.exitSingleViewSplittableEvent += ExitSingleView;
        rotationSpeed = InputsVR.singleton.GetComponent<ObjectSelection>().rotationSpeed;
    }

    private void EnterSingleView(Splittable splittable) {
        if(splittable == GetComponent<Splittable>()) {
            singleView = true;
        }
    }

    private void ExitSingleView(Splittable splittable) {
        if (splittable == GetComponent<Splittable>()) {
            singleView = false;
        }
    }

    // Update is called once per frame
    void Update () {
        if (singleView) {
            MouseDrag();
        }
	}
    private void MouseDrag() {
        if (Input.GetMouseButton(0)) {
            float rotX = Input.GetAxis("Mouse X") * rotationSpeed * Mathf.Rad2Deg;
            float rotY = Input.GetAxis("Mouse Y") * rotationSpeed * Mathf.Rad2Deg;
            transform.Rotate(Vector3.up, -rotX);
            transform.Rotate(Vector3.right, rotY);
        }
    }
}
