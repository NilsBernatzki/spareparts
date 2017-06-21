using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaceGhost : MonoBehaviour {

    public bool active;
    private MeshObject meshObject;
    private ChainObject chainObj;

    private void OnEnable() {
        
    }
    // Use this for initialization
    void Start () {
        meshObject = GetComponent<MeshObject>();
        chainObj = meshObject.chainObject;
    }
	// Update is called once per frame
	void Update () {
		
	}
    private void OnTriggerEnter(Collider other) {
        if (!active) return;
        if (!chainObj.linkedSocket) return;
        if (other.gameObject.layer == LayerMask.NameToLayer("Mesh")) {
            if (other.gameObject.GetComponent<MeshObject>().chainObject.mySockets.Contains(chainObj.linkedSocket)) {
                active = false;
                print(other.gameObject.name);
                SpawnGhost();
                SavePosition();
            }    
        }
    }
    private void SpawnGhost() {
        GameObject ghost = Instantiate(this.gameObject,chainObj.transform.position,transform.rotation);
        Destroy(ghost.GetComponent<Rigidbody>());
        Destroy(ghost.GetComponent<PlaceGhost>());
        Destroy(ghost.GetComponent<MeshRenderer>());
        ghost.GetComponent<MeshObject>().isGhost = true;
        chainObj.ghost = ghost;
    }
    private void SavePosition() {
        chainObj.endPos = chainObj.ghost.transform.position;
    }
}
