using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectObject : MonoBehaviour {

    public event System.Action<ChainObject> selectedChainObjectEvent;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        
        if (Input.GetMouseButtonDown(0)) {
            if (!SplitManager.singleton.splitted) return;
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out hit, 100.0f)) {
                if (hit.collider.gameObject.layer == LayerMask.NameToLayer("Mesh")) {
                    ChainObject chainObject = hit.collider.gameObject.GetComponent<MeshObject>().chainObject;
                    SelectChainObject(chainObject);
                }
            }
        }
    }
    private void SelectChainObject(ChainObject chainObject) {

        if(selectedChainObjectEvent != null) {
            selectedChainObjectEvent(chainObject);
        }
    }
}
