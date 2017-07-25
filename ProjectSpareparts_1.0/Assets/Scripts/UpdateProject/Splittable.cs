using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public enum SplittableState {
    start, end
}
[System.Serializable]
public struct SplittableTransforms {
    [SerializeField]
    public Vector3 position;
    [SerializeField]
    public Quaternion rotation;
}
[System.Serializable]
public class Splittable : MonoBehaviour {

    private SplittManager _splittManager;
    private Rigidbody _rig;
    private ObjectSelection _selection;
    private InteractableItem _thisItem;
    [SerializeField]
    public SplittableState _currentState;
    public int splitIndex;
    [SerializeField]
    public SplittableTransforms _startSet;
    [SerializeField]
    public SplittableTransforms _endSet;
    [SerializeField]
    public SplittableTransforms _currentSet;
   
    private float _splittSpeed;

	// Use this for initialization
	void Start () {
        Initialisation();
        _splittSpeed = _splittManager.globalSplitSpeed;
        ChangeCurrentTransformSet(_startSet);
	}
	void Initialisation() {
        _splittManager = SplittManager.singleton;
        _selection = InputsVR.singleton.GetComponent<ObjectSelection>();
        _splittManager.AddSplittable(this);
        _rig = GetComponent<Rigidbody>();
        _thisItem = GetComponent<InteractableItem>();
        gameObject.layer = LayerMask.NameToLayer("Splittable");
    }
	// Update is called once per frame
	void FixedUpdate () {
        if (!_selection.singleView) {
            if (!_thisItem.IsInteracting()) {
                Move(_currentSet, _splittSpeed);
            }
        }
	}

    public void Split() {
        ChangeCurrentTransformSet(_endSet);
        ChangeState(SplittableState.end);
    }

    public void Revert() {
        ChangeCurrentTransformSet(_startSet);
        ChangeState(SplittableState.start);
    }

    private void Move(SplittableTransforms currentTransformSet, float speed) {

        float timefdelta = Time.fixedDeltaTime;
        //Position
        Vector3 dir = currentTransformSet.position - _rig.position;
        float clampMagnitude = Vector3.ClampMagnitude(dir, 1).magnitude;
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
    private void ChangeState(SplittableState newState) {
        _currentState = newState;
    }
}
