using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectObject : MonoBehaviour {

    public event System.Action<ChainObject> selectedChainObjectEvent;
    public event System.Action<ChainObject> deselectChainObjectEvent; 
    public bool selectionMode;
    public ChainObject selectedChainObject;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
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
                }
            }
        }
        if (Input.GetMouseButtonDown(1)) {
            if (!SplitManager.singleton.splitted) return;
            if (!selectionMode) return;
            selectionMode = false;
            Debug.Log("Deselection");
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
}
