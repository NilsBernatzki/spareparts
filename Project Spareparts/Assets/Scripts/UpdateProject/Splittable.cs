using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public enum SplittableState {
    start, end, splitting
}
[System.Serializable]
public struct SplittableTransforms {
    public Vector3 position;
    public Quaternion rotation;
}

public class Splittable : MonoBehaviour {

    private SplittManager _splittManager;
    private Rigidbody _rig;

    [SerializeField]
    private int splitIndex;
    [SerializeField]
    private SplittableTransforms _startSet;
    [SerializeField]
    private SplittableTransforms _endSet;
    [SerializeField]
    private SplittableTransforms _currentSet;

    private SplittableState _currentState;

    [SerializeField]
    private float _splittSpeed;

	// Use this for initialization
	void Start () {
        Initialisation();
	}
	void Initialisation() {
        _splittManager = SplittManager.singleton;
        _splittManager.AddSplittable(this);
        _rig = GetComponent<Rigidbody>();
    }
	// Update is called once per frame
	void FixedUpdate () {
        Move(_currentSet,_splittSpeed);
	}

    public void Split() {
        ChangeCurrentTransformSet(_endSet);
    }

    public void Revert() {
        ChangeCurrentTransformSet(_startSet);
    }

    private void Move(SplittableTransforms currentTransformSet, float speed) {

        float timefdelta = Time.fixedDeltaTime;
        //Position
        Vector3 dir = currentTransformSet.position - _rig.position;
        float clampMagnitude = Vector3.ClampMagnitude(dir, 1f).magnitude;
        Vector3 movement = dir.normalized * speed * timefdelta * clampMagnitude;

        //Rotation
        Quaternion rot = Quaternion.RotateTowards(_rig.rotation, currentTransformSet.rotation, timefdelta * speed);

        _rig.rotation = rot;
        _rig.velocity = movement;
    }

    private void ChangeCurrentTransformSet(SplittableTransforms newTransformSet) {
        _currentSet.position = newTransformSet.position;
        _currentSet.rotation = newTransformSet.rotation;
    }
}
