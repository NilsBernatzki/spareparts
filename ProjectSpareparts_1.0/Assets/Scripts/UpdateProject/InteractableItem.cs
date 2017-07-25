using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractableItem : MonoBehaviour {

    private Rigidbody rig;
    private bool currentlyInteracting;
    private WandController attachedController;
    private Transform interactionPoint;

    private Vector3 posDelta;
    private float velocityFactor = 20000f;
    private float rotationFactor = 500f;
    private Quaternion rotationDelta;
    private float angle;
    private Vector3 axis;

	// Use this for initialization
	void Start () {
        rig = GetComponent<Rigidbody>();
        interactionPoint = new GameObject().transform;
        velocityFactor /= rig.mass;
        rotationFactor /= rig.mass;
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		if(attachedController && currentlyInteracting) {
            posDelta = attachedController.transform.position - interactionPoint.position;
            rig.velocity = posDelta * velocityFactor * Time.fixedDeltaTime;

            rotationDelta = attachedController.transform.rotation * Quaternion.Inverse(interactionPoint.rotation);
            rotationDelta.ToAngleAxis(out angle, out axis);

            if(angle > 180f) {
                angle -= 360f;
            }
            rig.angularVelocity = (Time.fixedDeltaTime * angle * axis) * rotationFactor;
        }
	}

    public void BeginInteraction(WandController controller) {
        attachedController = controller;
        interactionPoint.position = controller.transform.position;
        interactionPoint.rotation = controller.transform.rotation;
        interactionPoint.SetParent(transform, true);

        currentlyInteracting = true;
    }
    public void EndInteraction(WandController controller) {
        if(controller == attachedController) {
            attachedController = null;
            currentlyInteracting = false;
        }
    }
    public bool IsInteracting() {
        return currentlyInteracting;
    }
}
