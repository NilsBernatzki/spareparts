using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum KeyCodeVR
{
    trigger,
    grip,
    touch,

}

public enum ViveHand
{
    left,
    right,
}

public class GetInputVR : MonoBehaviour{
   
    public GameObject cameraRig;
    public bool vrControllersReady;
    public static GetInputVR singleton;


    private Valve.VR.EVRButtonId trigger = Valve.VR.EVRButtonId.k_EButton_SteamVR_Trigger;
    private Valve.VR.EVRButtonId grip = Valve.VR.EVRButtonId.k_EButton_Grip;
    private Valve.VR.EVRButtonId touch = Valve.VR.EVRButtonId.k_EButton_SteamVR_Touchpad;

    public SteamVR_TrackedObject[] trackedObj;


    // Use this for initialization
    void Awake()
    {
        singleton = this;
    }

    private void Start() {
        StartCoroutine(WaitForControllerSetup(2f));
    }
    private IEnumerator WaitForControllerSetup(float time) {

        vrControllersReady = false;
        yield return new WaitForSeconds(time);
        vrControllersReady = true;

    }
    /// <summary>
    /// Returns the gameObject of a given hand in the unity Scene
    /// </summary>
    /// <param name="whichHand"></param>
    /// <returns></returns>
    public GameObject GetControllerGameObject(ViveHand whichHand)
    {
        int handIndex = 0;
        if (whichHand == ViveHand.right)
        {
            handIndex = 0;
        }
        else handIndex = 1;



        return trackedObj[handIndex].gameObject;
    }

    /// <summary>
    /// Gets the input of the Vive Controller. 
    /// </summary>
    /// <param name="button"> use KeyCodeVR enum to determine which button.  </param>
    /// <param name="hand"> use ViveHand Enum to choose hand  </param>
    /// <returns></returns>
    public bool GetKeyDown(KeyCodeVR button, ViveHand hand) {

        bool toReturn = false;
        int handIndex = 0;

        if (hand == ViveHand.right) {
            handIndex = 1;
        } else handIndex = 0;

        if (SteamVR_Controller.Input((int)trackedObj[handIndex].index) == null) {
            return toReturn;
        }


        switch (button) {
            case KeyCodeVR.trigger:

                if (SteamVR_Controller.Input((int)trackedObj[handIndex].index).GetPressDown(trigger)) {

                    toReturn = true;
                } else {
                    toReturn = false;
                }

                break;


            case KeyCodeVR.grip:

                if (SteamVR_Controller.Input((int)trackedObj[handIndex].index).GetPressDown(grip)) {

                    toReturn = true;
                } else {
                    toReturn = false;
                }

                break;

            case KeyCodeVR.touch:

                if (SteamVR_Controller.Input((int)trackedObj[handIndex].index).GetPressDown(touch)) {
                    toReturn = true;
                } else {
                    toReturn = false;
                }

                break;
        }

        return toReturn;
    }

    /// <summary>
    /// Gets the input of the Vive Controller. 
    /// </summary>
    /// <param name="button"> use KeyCodeVR enum to determine which button.  </param>
    /// <param name="hand"> use ViveHand Enum to choose hand  </param>
    /// <returns></returns>
    public bool GetKeyUP(KeyCodeVR button, ViveHand hand)
    {
        bool toReturn = false;
        int handIndex = 0;

        if (hand == ViveHand.right)
        {
            handIndex = 0;
        }
        else handIndex = 1;

        if (SteamVR_Controller.Input((int)trackedObj[handIndex].index) == null)
        {
            return toReturn;
        }


        switch (button)

        {
            case KeyCodeVR.trigger:

                if (SteamVR_Controller.Input((int)trackedObj[handIndex].index).GetPressUp(trigger))
                {
                    toReturn = true;
                }
                else
                {
                    toReturn = false;
                }

                break;


            case KeyCodeVR.grip:

                if (SteamVR_Controller.Input((int)trackedObj[handIndex].index).GetPressUp(grip))
                {
                    toReturn = true;
                }
                else
                {
                    toReturn = false;
                }

                break;

            case KeyCodeVR.touch:

                if (SteamVR_Controller.Input((int)trackedObj[handIndex].index).GetPressUp(touch))
                {
                    toReturn = true;
                }
                else
                {
                    toReturn = false;
                }

                break;
        }

        return toReturn;
    }

    /// <summary>
    /// Gets the input of the Vive Controller. 
    /// </summary>
    /// <param name="button"> use KeyCodeVR enum to determine which button.  </param>
    /// <param name="hand"> use ViveHand Enum to choose hand  </param>
    /// <returns></returns>
    public bool GetKey(KeyCodeVR button, ViveHand hand)
    {
        bool toReturn = false;
        int handIndex = 0;

        if (hand == ViveHand.right)
        {
            handIndex = 0;
        }
        else handIndex = 1;

        if (SteamVR_Controller.Input((int)trackedObj[handIndex].index) == null)
        {
            return toReturn;
        }


        switch (button)

        {
            case KeyCodeVR.trigger:

                if (SteamVR_Controller.Input((int)trackedObj[handIndex].index).GetPress(trigger))
                {
                    
                    toReturn = true;
                }
                else
                {
                    toReturn = false;
                }

                break;


            case KeyCodeVR.grip:

                if (SteamVR_Controller.Input((int)trackedObj[handIndex].index).GetPress(grip))
                {
                    toReturn = true;
                }
                else
                {
                    toReturn = false;
                }

                break;

            case KeyCodeVR.touch:

                if (SteamVR_Controller.Input((int)trackedObj[handIndex].index).GetPress(touch))
                {
                    toReturn = true;
                }
                else
                {
                    toReturn = false;
                }

                break;
        }

        return toReturn;
    }

    public Vector2 GetAxis(ViveHand hand)
    {
        int handIndex = 0;
        Vector2 directionVector = Vector2.zero;
        if (hand == ViveHand.right)
        {
            handIndex = 0;
        }
        else handIndex = 1;

        if (SteamVR_Controller.Input((int)trackedObj[handIndex].index) == null)
        {
            return directionVector;
        }
        directionVector = SteamVR_Controller.Input((int)trackedObj[handIndex].index).GetAxis(touch);
        return directionVector;
    }
}
