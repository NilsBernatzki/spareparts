using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Turntable : MonoBehaviour {
    private ObjectSelection selection;

    public GameObject turnTable;
    public Splittable _selectedSplittable;
    public Camera mainCamera;
    [SerializeField]
    private float _speed;
	// Use this for initialization
	void Start () {
        selection = InputsVR.singleton.GetComponent<ObjectSelection>();
        selection.selectedSplittabletEvent += (s) => { _selectedSplittable = s; };
        selection.deselectSplittableEvent += (s) => { _selectedSplittable = null; };
	}
	
	// Update is called once per frame
	void Update () {
        float value = Input.GetAxis("Horizontal");
        if (value != 0) {
            if (_selectedSplittable) {
                mainCamera.transform.RotateAround(_selectedSplittable.transform.position, turnTable.transform.up, value * _speed * Time.deltaTime);
            } else {
                mainCamera.transform.RotateAround(turnTable.transform.position, turnTable.transform.up, value * _speed * Time.deltaTime);
            }
        }
	}
}
