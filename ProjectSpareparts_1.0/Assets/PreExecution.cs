using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PreExecution : MonoBehaviour {
    public void SetStartPositionOfSpareParts() {
        Debug.Log("Start Saved");
        Splittable[] splittables = GetComponentsInChildren<Splittable>();
        for (int i = 0; i < splittables.Length; i++) {
            
            splittables[i]._startSet.position = splittables[i].transform.position;
            splittables[i]._startSet.rotation = splittables[i].transform.rotation;

        }
    }

    public void SetEndPositionOfSpareParts() {
        Debug.Log("End Saved");
        Splittable[] splittables = GetComponentsInChildren<Splittable>();
        for(int i = 0; i < splittables.Length; i++) {

            splittables[i]._endSet.position = splittables[i].transform.position;
            splittables[i]._endSet.rotation = splittables[i].transform.rotation;
           
        }
    }
    
    public void SetObjectsToStartPos() {
        Splittable[] splittables = GetComponentsInChildren<Splittable>();
        for (int i = 0; i < splittables.Length; i++) {
            splittables[i].transform.position = splittables[i]._startSet.position;
            splittables[i].transform.rotation = splittables[i]._startSet.rotation;
        }

    }
    public void SetObjectsToEndPos() {
        Splittable[] splittables = GetComponentsInChildren<Splittable>();
        for (int i = 0; i < splittables.Length; i++) {
            splittables[i].transform.position = splittables[i]._endSet.position;
            splittables[i].transform.rotation = splittables[i]._endSet.rotation;
        }
    }
}
