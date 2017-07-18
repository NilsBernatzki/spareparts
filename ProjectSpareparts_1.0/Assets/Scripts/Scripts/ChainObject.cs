using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(Rotate))]
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

    [Header("Selection")]
    public bool isSelected;

    void Start () {
        GameManager.singleton.GetComponent<SelectObject>().selectedChainObjectEvent += OnSelection;
        GameManager.singleton.GetComponent<SelectObject>().deselectChainObjectEvent += OnDeselection;
    }
	
	// Update is called once per frame
	void Update () {
        if (!isRoot) {
            attachPoint.debugLink = debugLink;
        }

	}
    public void SetValues() {
        startPos = transform.position;
        endPos = startPos + splitVector * 10f;
    }
    private void OnDeselection(ChainObject chainObject) {
        if(chainObject == this) {
            if (isSelected) {
                isSelected = false;
            } else {
                Debug.Log("deselect a deselected Object? :/ ");
            }
        }
    }
    private void OnSelection(ChainObject chainObject) {
        if (chainObject == this) {
            if (!isSelected) {
                isSelected = true;
                //Debug.Log("selected: " + this.gameObject.name, this);
            }
        } else {
            if (isSelected) {
                isSelected = false;
                //Debug.Log("??");
            }
        }
    }
}
