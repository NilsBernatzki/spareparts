using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Turntable : MonoBehaviour {

    public GameObject turnTable;
    public Camera mainCamera;
    [SerializeField]
    private float _speed;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        float value = Input.GetAxis("Horizontal");
        if (value != 0) {
            mainCamera.transform.RotateAround(turnTable.transform.position, turnTable.transform.up, value * _speed * Time.deltaTime);
        }
	}
}
