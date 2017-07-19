﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectSelection : MonoBehaviour {

    public event System.Action<Splittable> selectedSplittabletEvent;
    public event System.Action<Splittable> deselectSplittableEvent;
    public event System.Action<Splittable> enterSingleViewSplittableEvent;
    public event System.Action<Splittable> exitSingleViewSplittableEvent;

    public Camera mainCamera;
    private Splittable _selectedSplittable;
    private bool selection;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetMouseButtonDown(0)) {
            FireRayToScreenPos();
            return;
        }
        if (Input.GetMouseButtonDown(1)) {
            if (selection) {
                deselectSplittableEvent(_selectedSplittable);
                _selectedSplittable = null;
                selection = false;
            }
            
        }
	}
    private void FireRayToScreenPos() {
        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out hit, 100.0f, 1 << LayerMask.NameToLayer("Splittable"))) {

            Splittable splittable = hit.collider.gameObject.GetComponent<Splittable>();
            if (splittable._currentState == SplittableState.end) {
                _selectedSplittable = splittable;
                selection = true;
                selectedSplittabletEvent(_selectedSplittable);
            }
        }
    }
}
