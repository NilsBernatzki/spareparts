using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;

public class GameManager : MonoBehaviour {
    public static GameManager singleton;
    public bool startedTool;
    public bool finishedSetup;
    
    void Awake() {
        singleton = this;
    }

	// Use this for initialization
	void Start () {
		
	}

    // Update is called once per frame
    void Update() {
        if (Input.GetKeyDown(KeyCode.Space)) {
            if (!ObjectManager.singleton.model) {
                //GetComponent<MeshImporter>().LoadInModels();
            }
        }
        if (Input.GetKeyDown(KeyCode.LeftControl) && !startedTool) {
            startedTool = true;
            StartTool();
        }
    }
   
    private void StartTool() {
        StartCoroutine(StartManagerCoroutine());
    }
    private IEnumerator StartManagerCoroutine() {
        ObjectManager.singleton.SetUp();
        yield return new WaitUntil(() => ObjectManager.singleton.finishedObjectSetup);
        SplitManager.singleton.SetUp();
        yield return new WaitUntil(() => SplitManager.singleton.finishedStartSplit);
        finishedSetup = true;
    }
}
