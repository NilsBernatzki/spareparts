using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WandController : MonoBehaviour {

    private List<InteractableItem> objectsHoveringOver = new List<InteractableItem>();
    InteractableItem closestItem = null;
    InteractableItem interactingItem = null;

    InputsVR _inputs;

    private bool interact;
    private bool stopInteract;

    // Use this for initialization
    void Start () {
        _inputs = InputsVR.singleton;
        _inputs.Grab += (c) => { if (c == this) interact = true; };
        _inputs.Release += (c) => { if (c == this) stopInteract = true; };
	}
	
	// Update is called once per frame
	void Update () {
        if (interact) {
            interact = false;
            InteractWithItem();
        }
        if (stopInteract) {
            stopInteract = false;
            StopInteractWithItem();
        }
    }

    public void InteractWithItem() {
        closestItem = null;
        float minDistance = float.MaxValue;
        float distance;

        foreach (InteractableItem item in objectsHoveringOver) {
            
            distance = (item.transform.position - transform.position).sqrMagnitude;
            if (distance < minDistance) {
                minDistance = distance;
                closestItem = item;
            }
        }
        interactingItem = closestItem;

        if (interactingItem) {
            if (interactingItem.IsInteracting()) {
                interactingItem.EndInteraction(this);
            }
            
            interactingItem.BeginInteraction(this);
        } 
    }
    public void StopInteractWithItem() {
        if (interactingItem) {
            interactingItem.EndInteraction(this);
        }
    }

    private void OnTriggerEnter(Collider other) {
        InteractableItem collidingItem = other.GetComponent<InteractableItem>();
        if (collidingItem) {
            objectsHoveringOver.Add(collidingItem);
        }
    }

    private void OnTriggerExit(Collider other) {
        InteractableItem collidingItem = other.GetComponent<InteractableItem>();
        if (collidingItem) {
            objectsHoveringOver.Remove(collidingItem);
        }
    }
}
