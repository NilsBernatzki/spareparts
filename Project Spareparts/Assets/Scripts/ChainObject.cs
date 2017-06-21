using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChainObject : MonoBehaviour {
    public PlaceGhost placeGhost;
    public bool isRoot;
    public Attachpoint attachPoint;
    public Socket linkedSocket;
    public List<Socket> mySockets = new List<Socket>();
    public GameObject meshObject;
    public GameObject ghost;
    [SerializeField]
    private bool debugLink = true;

    [Header("Split")]
    public Vector3 splitVector;
    public Vector3 startPos;
    //public Quaternion startRot;
    public Vector3 endPos;
    //public Quaternion endRot;
    
    private void OnEnable() {
        startPos = transform.position;
    }

    void Start () {
        
	}
	
	// Update is called once per frame
	void Update () {
        if (!isRoot) {
            attachPoint.debugLink = debugLink;
        }

	}
}
