using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attachpoint : MonoBehaviour {

    [HideInInspector]
    public bool debugLink;
    public ChainObject chainObj;
	// Use this for initialization
	void Start () {
        
	}
	
	// Update is called once per frame
	void Update () {
        if (debugLink && chainObj.linkedSocket) {
            DrawLink(); 
        }
	}
    public void LinkToSocket() {
        chainObj = transform.parent.GetComponent<ChainObject>();
        RaycastHit hit;
        BoxCollider coll = GetComponent<BoxCollider>();
        Vector3 center = transform.position - transform.up * coll.bounds.extents.y;
        Vector3 dir = transform.up;
        float length = coll.bounds.extents.y;
        LayerMask mask = ObjectManager.singleton.socketLayer;

        if(Physics.Raycast(center,dir,out hit, length, mask)){
            if(chainObj == null) {
                print("test");
            }
            chainObj.linkedSocket = hit.collider.gameObject.GetComponent<Socket>();
            chainObj.linkedSocket.GetComponent<Socket>().attachedAttachPoint = this;
            //Set Split Vec on socket rotation
            SetSplitVector(chainObj.linkedSocket);
        }
    }
    private void DrawLink() {
        Debug.DrawRay(transform.position, chainObj.linkedSocket.transform.position - transform.position, Color.red);
    }
    private void SetSplitVector(Socket socket) {
        transform.parent.GetComponent<ChainObject>().splitVector = socket.transform.up;
    }
}
