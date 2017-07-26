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

    [SerializeField]
    private Color hoverOverColor;

    private WandController otherWand;
    // Use this for initialization
    void Start () {
        _inputs = InputsVR.singleton;
        GetInputVR.singleton.wands.Add(this);
        _inputs.Grab += (c) => { if (c == this) interact = true; };
        _inputs.Release += (c) => { if (c == this) stopInteract = true; };
        StartCoroutine(SetOtherWand());
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
        SetClosestInteractableItem(ref closestItem);
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
    private void SetClosestInteractableItem(ref InteractableItem closestItem) {
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
    }
    private void OnTriggerEnter(Collider other) {
        InteractableItem collidingItem = other.GetComponent<InteractableItem>();
        if (collidingItem) {
            objectsHoveringOver.Add(collidingItem);
            SetClosestInteractableItem(ref closestItem);
            ChangeColorOfInteractables();
        }
    }

    private void OnTriggerExit(Collider other) {
        InteractableItem collidingItem = other.GetComponent<InteractableItem>();
        if (collidingItem) {
            objectsHoveringOver.Remove(collidingItem);
            if (collidingItem != otherWand.GetClosestItem() && collidingItem.IsInteracting() == false) {
                collidingItem.ResetColor();
            }
            if (collidingItem.IsInteracting()) {
                StartCoroutine(ChangeColorDelayed(collidingItem));
            }
            SetClosestInteractableItem(ref closestItem);
            ChangeColorOfInteractables();
        }
    }
    private void ChangeColorOfInteractables() {
        foreach (InteractableItem item in objectsHoveringOver) {
            if(item == closestItem) {
                item.ChangeColorHoverOver(hoverOverColor);
            } else {
                if(item != otherWand.GetClosestItem()) {
                    item.ResetColor();
                }
            }
        }
    }
    private IEnumerator SetOtherWand() {

        yield return new WaitUntil(() => GetInputVR.singleton.wands.Count == 2);
        foreach(WandController wand in GetInputVR.singleton.wands) {
            if(wand != this) {
                otherWand = wand;
            }
        }
    }
    public InteractableItem GetClosestItem() {
        return closestItem;
    }

    private IEnumerator ChangeColorDelayed(InteractableItem item) {
        yield return new WaitUntil(() => !item.IsInteracting());
        item.ResetColor();
    }
}
