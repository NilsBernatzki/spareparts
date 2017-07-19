using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FocusSplittable : MonoBehaviour {

    private ObjectSelection selection;
    public Splittable _selectedSplittable;

    private bool focused;
    public float animationSpeed;
    public float minDistToSelection;
    [Range(0, 1)]
    public float alpha;

    // Use this for initialization
    void Start() {
        selection = InputsVR.singleton.GetComponent<ObjectSelection>();
        selection.selectedSplittabletEvent += FocusSelectedChainObject;
        selection.deselectSplittableEvent += DefocusSelectedChainObject;
    }

    private void Update() {
        if (_selectedSplittable) {
            RotateTowardsSelection();
            FollowSelection();
        } else {

        }
    }

    private void FocusSelectedChainObject(Splittable splittable) {
        StartCoroutine(TransparentAll());
        _selectedSplittable = splittable;
    }

    private void RotateTowardsSelection() {
        transform.rotation = Quaternion.RotateTowards(transform.rotation,
           Quaternion.LookRotation(_selectedSplittable.transform.position - transform.position),
           Time.deltaTime * animationSpeed);
    }
    private void FollowSelection() {
        if(Vector3.Distance(_selectedSplittable.transform.position, transform.position) > minDistToSelection) {
            Vector3 dir = _selectedSplittable.transform.position - transform.position;
            float clampMagnitude = Vector3.ClampMagnitude(dir, 1).magnitude;
            Vector3 movement = dir * animationSpeed/100 * Time.deltaTime * clampMagnitude;
            transform.position += movement;
        }
    }
   
    private void DefocusSelectedChainObject(Splittable splittable) {
        StartCoroutine(UnTransparentAll());
        _selectedSplittable = null;
    }
    private IEnumerator ResetRotation() {
        yield return null;
    }
    private IEnumerator ResetPosition() {
        yield return null;
    }
    private IEnumerator TransparentAll() {
       foreach(Splittable splittable in SplittManager.singleton._allSplittablesList) {
            if(splittable != _selectedSplittable) {
                splittable.GetComponent<ChangeColor>().Transparent(alpha);
                yield return null;
            }
        }
    }
    private IEnumerator UnTransparentAll() {
        foreach (Splittable splittable in SplittManager.singleton._allSplittablesList) {
            if (splittable != _selectedSplittable) {
                splittable.GetComponent<ChangeColor>().Untransparent();
                yield return null;
            }
        }
    }
}
