﻿using UnityEngine;
using System.Collections;

public class WandController : MonoBehaviour {

	public GameObject menu;
	public GameObject cube;
	public float scaleFactor;

	private SteamVR_TrackedObject trackedObj;
	private Valve.VR.EVRButtonId gripButton = Valve.VR.EVRButtonId.k_EButton_Grip;
	private Valve.VR.EVRButtonId menuButton = Valve.VR.EVRButtonId.k_EButton_ApplicationMenu;
	private Valve.VR.EVRButtonId triggerButton = Valve.VR.EVRButtonId.k_EButton_SteamVR_Trigger;
	private Valve.VR.EVRButtonId touchPadUp = Valve.VR.EVRButtonId.k_EButton_DPad_Up;

	SteamVR_Controller.Device controllerMain;
    SteamVR_Controller.Device controllerSecondary;

    private bool menuButtonDown;
	private bool showMenu;
    private bool gripIsPressed;
    private bool bothTriggersPressed;
    private float oldControllerDistance = float.NaN;

	private GameObject selected;
	private GameObject grabbed;

	private Gizmo gizmoControl;
	// Use this for initialization
	void Start () {
		trackedObj = GetComponent<SteamVR_TrackedObject> ();
		menu.SetActive (false);
		menuButtonDown = false;
		showMenu = false;

        setupControllers();
    }

	// Update is called once per frame
	void Update () {
		//SteamVR_Controller.Device controller = SteamVR_Controller.Input ((int)trackedObj.index);

		if (controllerMain == null) {
			Debug.Log ("Controller not initialized");
			return;
		}

		menuButtonDown = controllerMain.GetPressDown (menuButton);

		if (menuButtonDown) {
			showMenu = !showMenu;
			menu.SetActive (showMenu);
		}

		if (controllerMain.GetPressDown (triggerButton) && selected != null) {
			grabbed = selected;
			grabbed.transform.SetParent (this.transform);
			grabbed.GetComponent<Rigidbody> ().isKinematic = true;
		}
		if (controllerMain.GetPressUp (triggerButton) && selected != null) {
			grabbed.transform.SetParent (null);
			grabbed.GetComponent<Rigidbody> ().isKinematic = false;
			grabbed = null;
		}



        //if (controller.GetPressDown(SteamVR_Controller.ButtonMask.Touchpad) && selected != null)
        //{
        //    Debug.Log("Touchpad pressed");
        //    if (controller.GetAxis(Valve.VR.EVRButtonId.k_EButton_SteamVR_Touchpad).y > 0.5f)
        //    {
        //        Debug.Log("Dpad Up");
        //        float scale = Time.deltaTime;
        //        selected.transform.localScale += new Vector3(scaleFactor, scaleFactor, scaleFactor);
        //    }
        //    if (controller.GetAxis(Valve.VR.EVRButtonId.k_EButton_SteamVR_Touchpad).y < -0.5)
        //    {
        //        Debug.Log("Dpad Down");
        //        scaleDown(selected);
        //        selected.transform.localScale -= new Vector3(scaleFactor, scaleFactor, scaleFactor);
        //    }
        //}

        if (controllerSecondary.GetPressDown(Valve.VR.EVRButtonId.k_EButton_Grip))
        {
            gripIsPressed = true;
        }
        if (controllerSecondary.GetPressUp(Valve.VR.EVRButtonId.k_EButton_Grip))
        {
            gripIsPressed = false;
        }

        if (controllerSecondary.GetPressDown(triggerButton))
        {
            bothTriggersPressed = true;
        }
        if (controllerSecondary.GetPressUp(triggerButton))
        {
            bothTriggersPressed = false;
        }

        if (gripIsPressed && selected != null)
        {
            scaleSelected(selected);
        }

        if (bothTriggersPressed && selected != null)
        {
            enlargeSelected(selected);
        }
    }
    void OnTriggerEnter(Collider collider) {
		selected = collider.gameObject;
        setupControllers();
	}

	void OnTriggerExit(Collider collider) {
		selected = null;
		Transform oldCubeTransform = this.transform.Find ("MenuCube");
		Transform oldSphereTransform = this.transform.Find ("MenuSphere");
		if (oldCubeTransform != null) {
			oldCubeTransform.SetParent (null);
		}
		if (oldSphereTransform != null) {
			oldSphereTransform.SetParent (null);
		}
	}

    void scaleSelected(GameObject selected)
    {
        Vector3 velocity = controllerSecondary.velocity;
        Vector3 newScale = Vector3.zero;
        float scalingFactor = 2.0f;

        float max = Mathf.Max(Mathf.Max(Mathf.Abs(velocity.x), Mathf.Abs(velocity.y)), Mathf.Abs(velocity.z));
        
        if(max == Mathf.Abs(velocity.x))
        {
            newScale = selected.transform.localScale + new Vector3(velocity.x * scalingFactor, 0, 0);
        }
        else if(max == Mathf.Abs(velocity.y))
        {
            newScale = selected.transform.localScale + new Vector3(0, velocity.y * scalingFactor, 0);
        }
        else if (max == Mathf.Abs(velocity.z))
        {
            newScale = selected.transform.localScale + new Vector3(0, 0, velocity.z * scalingFactor);
        }

        selected.transform.localScale = Vector3.Lerp(selected.transform.localScale, newScale, Time.deltaTime);
    }

    void enlargeSelected(GameObject selected)
    {
        
        Vector3 velocityA = controllerMain.velocity;
        Vector3 velocityB = controllerSecondary.velocity;
        float delta = Vector3.Dot(velocityA, velocityB);
        //float delta = Vector3.Distance(velocityB, velocityA);

        selected.transform.localScale = Vector3.Lerp(selected.transform.localScale, 2*delta * selected.transform.localScale, Time.deltaTime);
    }

    void setupControllers()
    {
        if ((int)trackedObj.index == SteamVR_Controller.GetDeviceIndex(SteamVR_Controller.DeviceRelation.Leftmost))
        {
            controllerMain = SteamVR_Controller.Input((int)trackedObj.index);
            controllerSecondary = SteamVR_Controller.Input(SteamVR_Controller.GetDeviceIndex(SteamVR_Controller.DeviceRelation.Rightmost));
        }
        else
        {
            controllerMain = SteamVR_Controller.Input((int)trackedObj.index);
            controllerSecondary = SteamVR_Controller.Input(SteamVR_Controller.GetDeviceIndex(SteamVR_Controller.DeviceRelation.Leftmost));

        }
    }
}