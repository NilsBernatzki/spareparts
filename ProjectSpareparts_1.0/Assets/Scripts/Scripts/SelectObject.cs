﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectObject : MonoBehaviour {

    public event System.Action<ChainObject> selectedChainObjectEvent;
    public event System.Action<ChainObject> deselectChainObjectEvent;
    public event System.Action<ChainObject> enterSingleViewChainObjectEvent;
    public event System.Action<ChainObject> exitSingleViewChainObjectEvent;

    public bool selectionMode;
    public ChainObject selectedChainObject;
    public bool singleView;
    public Camera mainCamera;
   
    // Use this for initialization
    void Start() {

    }

    // Update is called once per frame
    void Update() {

        if (selectionMode) {
            if (Input.GetKeyDown(KeyCode.S)) {
                if (selectedChainObject != null) {
                    if (!singleView) {
                        singleView = true;
                        EnterSingleView(selectedChainObject);
                        return;
                    } else {
                        singleView = false;
                        ExitSingleView(selectedChainObject);
                        return;
                    }
                }
            }
        }
        if (singleView) return;
        if (Input.GetMouseButtonDown(0)) {
            if (!SplitManager.singleton.splitted) return;
            if (selectionMode) return;
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out hit, 100.0f)) {
                if (hit.collider.gameObject.layer == LayerMask.NameToLayer("Mesh")) {
                    selectionMode = true;
                    ChainObject chainObject = hit.collider.gameObject.GetComponent<MeshObject>().chainObject;
                    selectedChainObject = chainObject;
                    SelectChainObject(chainObject);
                    mainCamera.GetComponent<FocusSelection>().selectedChainObject = chainObject;
                }
            }
        }
        if (Input.GetMouseButtonDown(1)) {
            if (!SplitManager.singleton.splitted) return;
            if (!selectionMode) return;
            selectionMode = false;
            selectedChainObject = null;
            DeselectChainObject();
            mainCamera.GetComponent<FocusSelection>().selectedChainObject = null;
        }
    }
    
   
    private void DeselectChainObject() {
        if(deselectChainObjectEvent != null) {
            deselectChainObjectEvent(selectedChainObject);
        }
    }
    private void SelectChainObject(ChainObject chainObject) {
        if(selectedChainObjectEvent != null) {
            selectedChainObjectEvent(chainObject);
        }
    }
    private void EnterSingleView(ChainObject selectedChainObject) {
        if(enterSingleViewChainObjectEvent != null) {
            enterSingleViewChainObjectEvent(selectedChainObject);
        }
    }
    private void ExitSingleView(ChainObject selectedChainObject) {
        if(exitSingleViewChainObjectEvent != null) {
            exitSingleViewChainObjectEvent(selectedChainObject);
        }
    }
}
