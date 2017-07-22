using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FocusSplittable : MonoBehaviour {

    private ObjectSelection selection;
    public Splittable _selectedSplittable;

    public Transform focusEmptyTransform;
    private float standardDist;
    [SerializeField]
    private float minDistToSelection;

    private bool focused;
    private bool singleView;
    public float animationSpeed;
    [Range(0, 1)]
    public float alpha;

    // Use this for initialization
    void Start() {
        selection = InputsVR.singleton.GetComponent<ObjectSelection>();
        selection.selectedSplittabletEvent += FocusSelectedSplittable;
        selection.deselectSplittableEvent += DefocusSelectedSplittable;
        selection.enterSingleViewSplittableEvent += EnterSingleView;
        selection.exitSingleViewSplittableEvent += ExitSingleView;
        standardDist = Vector3.Distance(focusEmptyTransform.position, transform.position);
    }

    private void Update() {
        if (_selectedSplittable) {
            RotateTowardsSelection(_selectedSplittable.transform.position);
            FollowSelection(_selectedSplittable.transform.position,minDistToSelection);
        } else {
            FollowSelection(focusEmptyTransform.position, standardDist);
            RotateTowardsSelection(focusEmptyTransform.position);
        }
    }

    private void FocusSelectedSplittable(Splittable splittable) {
        _selectedSplittable = splittable;
        StartCoroutine(TransparentAll());
        focused = true;
    }

    private void RotateTowardsSelection(Vector3 position) {
        transform.rotation = Quaternion.RotateTowards(transform.rotation,
       Quaternion.LookRotation(position - transform.position),
       Time.deltaTime * animationSpeed);
    }
    private void FollowSelection(Vector3 position,float distance) {
        Vector3 dir;

        dir = (position +
       (transform.position - position).normalized * distance) -
       transform.position;

        float clampMagnitude = Vector3.ClampMagnitude(dir, 1).magnitude;
        Vector3 movement = new Vector3(dir.x, 0, dir.z) * animationSpeed / 4f * Time.deltaTime;
        transform.position += movement;
    }

    private void DefocusSelectedSplittable(Splittable splittable) {
        _selectedSplittable = null;
        StartCoroutine(UnTransparentAll());
        focused = false;
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
                if (singleView) {
                    splittable.GetComponent<ChangeColor>().Transparent(0);
                } else {
                    splittable.GetComponent<ChangeColor>().Transparent(alpha);
                }
                yield return null;
            } else {
                splittable.GetComponent<ChangeColor>().Untransparent();
            }
        }
        if (!singleView) {
            SplittManager.singleton.root.GetComponent<ChangeColor>().Transparent(alpha);
        } else {
            SplittManager.singleton.root.GetComponent<ChangeColor>().Transparent(0);
        }
        
    }
    private IEnumerator UnTransparentAll() {
        foreach (Splittable splittable in SplittManager.singleton._allSplittablesList) {
            splittable.GetComponent<ChangeColor>().Untransparent();
            yield return null;
        }
        SplittManager.singleton.root.GetComponent<ChangeColor>().Untransparent();
    }
    private void EnterSingleView(Splittable splittable) {
        singleView = true;
        StartCoroutine(TransparentAll());
    }
    private void ExitSingleView(Splittable splittable) {
        singleView = false;
        StartCoroutine(TransparentAll());
    }
}
