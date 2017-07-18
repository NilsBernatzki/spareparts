using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SocketSorter : IComparer<Socket> {
    public int Compare(Socket x, Socket y) {
        return x.socketIndex.CompareTo(y.socketIndex);
    }
}

public class ObjectManager : MonoBehaviour {
    public static ObjectManager singleton;

    public GameObject model;
    public LayerMask socketLayer;
    private List<GameObject> sparepartList = new List<GameObject>();
    private List<Attachpoint> attachPointList = new List<Attachpoint>();
    public List<GameObject> meshes = new List<GameObject>();
    public List<Socket> socketList = new List<Socket>();
    public float capsuleColMeshAdd;
    public float minCollHeigth;
    public bool finishedObjectSetup;

    void Awake() {
        singleton = this;
    }
    // Use this for initialization
    void Start () {
        
	}
	
	// Update is called once per frame
	void Update () {
		
	}
    public void SetUp() {
        PrepareSpareParts();
        LinkAttachpointsToSockets();
        SortSockets();
        ChainObjectSetup();
        finishedObjectSetup = true;
    }
    private void ChainObjectSetup() {
        ChainObject[] chainObjects = model.GetComponentsInChildren<ChainObject>();
        foreach(ChainObject chainObject in chainObjects) {
            chainObject.SetValues();
        }
    }
    /// <summary>
    /// Detaches all children in model, instatiates new container and parents children
    /// with same index to new container, adds scripts and colliders, hide
    /// </summary>
    public void PrepareSpareParts() {
        Transform tModel = model.transform;
        for(int i = 0; i< tModel.childCount; i++) {
            sparepartList.Add(tModel.GetChild(i).gameObject);
        }
        int currentIndex = -1;
        GameObject newContainer = null;
        foreach (GameObject part in sparepartList) {
            int objectIndex = GetObjectIndex(part.name);
            if (currentIndex != objectIndex) {
                newContainer = new GameObject(GetName(part.name));
                newContainer.transform.position = part.transform.position;
                newContainer.AddComponent<ChainObject>();
                newContainer.AddComponent<Rigidbody>();
                newContainer.GetComponent<Rigidbody>().useGravity = false;
                newContainer.transform.parent = tModel;
                currentIndex = objectIndex;
                if (currentIndex == 0) {
                    newContainer.GetComponent<ChainObject>().isRoot = true;
                } else {
                    newContainer.AddComponent<Split>();
                    newContainer.GetComponent<Split>().SetFields();
                }
            }
            if(currentIndex == objectIndex) {
                part.transform.parent = newContainer.transform;
                AddScripts(part);
                AddColliderAndRig(part);
                
            }
        }
    }
    /// <summary>
    /// return int index of object based on name set in blender
    /// index is first string in splitted array of object name
    /// </summary>
    /// <param name="objectName">set in blender</param>
    /// <returns></returns>
    private int GetObjectIndex(string objectName) {
        char delimiter = '_';
        string[] strings = objectName.Split(delimiter);
        return int.Parse(strings[0]);
    }
    /// <summary>
    /// returns name of object
    /// </summary>
    /// <param name="objectName">set in blender</param>
    /// <returns>object name set in blender last part of splitted string array</returns>
    private string GetName(string objectName) {
        char delimiter = '_';
        string[] strings = objectName.Split(delimiter);
        return strings[strings.Length-1];
    }
    /// <summary>
    /// return int AttachmentIndex of object based on name set in blender
    /// Attachmentindex is second string in splitted array of object name
    /// 0 = attachpoint , 1 = socket, 2 = mesh -> no script added
    /// </summary>
    /// <param name="objectName">set in blender</param>
    /// <returns></returns>
    private int GetAttachmentIndex(string objectName) {
        char delimiter = '_';
        string[] strings = objectName.Split(delimiter);
        return int.Parse(strings[1]);
    }
    /// <summary>
    /// return int SocketIndex of object based on name set in blender
    /// Attachmentindex is second string in splitted array of object name
    /// index set order for split 
    /// </summary>
    /// <param name="objectName">set in blender</param>
    /// <returns></returns>
    private int GetSocketIndex(string objectName) {
        char delimiter = '_';
        string[] strings = objectName.Split(delimiter);
        return int.Parse(strings[3]);
    }
    /// <summary>
    /// Adds attachpoint script or socket script to socket and ap
    /// adds to lists
    /// links attachpoint to parent chainObject
    /// sets index of socket and moves to socket layer
    /// </summary>
    /// <param name="part"></param>
    private void AddScripts(GameObject part) {
        int attachIndex = GetAttachmentIndex(part.name);
        if (attachIndex == 0) {
            part.AddComponent<Attachpoint>();
            part.transform.parent.GetComponent<ChainObject>().attachPoint = part.GetComponent<Attachpoint>();
            //Add to list
            attachPointList.Add(part.GetComponent<Attachpoint>());
        }
        if (attachIndex == 1) {
            part.AddComponent<Socket>();
            part.GetComponent<Socket>().socketIndex = GetSocketIndex(part.name);
            MoveToLayer(part,"Socket");
            //Add to list
            socketList.Add(part.GetComponent<Socket>());
            part.transform.parent.GetComponent<ChainObject>().mySockets.Add(part.GetComponent<Socket>());
        }
        if(attachIndex == 2) {
            MoveToLayer(part, "Mesh");
            part.transform.parent.GetComponent<ChainObject>().meshObject = part;
            part.AddComponent<PlaceGhost>();
            part.AddComponent<MeshObject>();
            part.GetComponent<MeshObject>().chainObject = part.transform.parent.GetComponent<ChainObject>();
            meshes.Add(part);
        }
    }
    private void MoveToLayer(GameObject part, string layerName) {
        part.layer = LayerMask.NameToLayer(layerName);
    }
    /// <summary>
    /// adds collider to ap and socket and disable MeshRenderer
    /// </summary>
    /// <param name="part"></param>
    private void AddColliderAndRig(GameObject part) {
        int attachIndex = GetAttachmentIndex(part.name);
        if(attachIndex != 2) {
            part.AddComponent<BoxCollider>();
            part.GetComponent<BoxCollider>().isTrigger = true;
            part.GetComponent<MeshRenderer>().enabled = false;
        } else {
            part.AddComponent<CapsuleCollider>();
            part.GetComponent<CapsuleCollider>().isTrigger = true;
            part.GetComponent<CapsuleCollider>().height *= capsuleColMeshAdd;
            if(part.GetComponent<CapsuleCollider>().height < minCollHeigth) {
                part.GetComponent<CapsuleCollider>().height = minCollHeigth;
            }
            part.GetComponent<CapsuleCollider>().radius *= capsuleColMeshAdd;
            if (!part.GetComponent<MeshObject>().chainObject.isRoot) {
                //part.GetComponent<MeshRenderer>().enabled = false;
            }
        }
    }
    /// <summary>
    /// Calls link method in each attachpoint to link to socket the attachpoin is colling with
    /// </summary>
    private void LinkAttachpointsToSockets() {
        foreach(Attachpoint ap in attachPointList) {
            ap.LinkToSocket();
        }
    }
    /// <summary>
    /// Sorts Sockets on Socketindex
    /// </summary>
    private void SortSockets() {
        SocketSorter sSorter = new SocketSorter();
        socketList.Sort(sSorter);
    }
}
