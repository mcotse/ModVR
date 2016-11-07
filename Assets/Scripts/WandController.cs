using UnityEngine;
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
	SteamVR_Controller.Device controller;
    SteamVR_Controller.Device controllerRight;
    SteamVR_Controller.Device controllerLeft;

    private bool menuButtonDown;
	private bool showMenu;
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

		controller = SteamVR_Controller.Input ((int)trackedObj.index);
        controllerLeft = SteamVR_Controller.Input(SteamVR_Controller.GetDeviceIndex(SteamVR_Controller.DeviceRelation.Leftmost));
        controllerRight = SteamVR_Controller.Input(SteamVR_Controller.GetDeviceIndex(SteamVR_Controller.DeviceRelation.Rightmost));
    }

	// Update is called once per frame
	void Update () {
		//SteamVR_Controller.Device controller = SteamVR_Controller.Input ((int)trackedObj.index);

		if (controller == null) {
			Debug.Log ("Controller not initialized");
			return;
		}

		menuButtonDown = controller.GetPressDown (menuButton);

		if (menuButtonDown) {
			showMenu = !showMenu;
			menu.SetActive (showMenu);
		}

		if (controller.GetPressDown (triggerButton) && selected != null) {
			grabbed = selected;
			grabbed.transform.SetParent (this.transform);
			grabbed.GetComponent<Rigidbody> ().isKinematic = true;
		}
		if (controller.GetPressUp (triggerButton) && selected != null) {
			grabbed.transform.SetParent (null);
			grabbed.GetComponent<Rigidbody> ().isKinematic = false;
			grabbed = null;
		}

        if(controllerRight.GetPressDown(Valve.VR.EVRButtonId.k_EButton_Grip) && controllerLeft.GetPressDown(Valve.VR.EVRButtonId.k_EButton_Grip))
        {
            scaleSelected(selected);
        }
        else
        {
            oldControllerDistance = float.NaN;
        }

        //if (controller.GetPressDown(SteamVR_Controller.ButtonMask.Touchpad) && selected != null)
        //{
        //    Debug.Log("Touchpad pressed");
        //    if (controller.GetAxis(Valve.VR.EVRButtonId.k_EButton_SteamVR_Touchpad).y > 0.5f)
        //    {
        //        Debug.Log("Dpad Up");
        //        selected.transform.localScale += new Vector3(scaleFactor, scaleFactor, scaleFactor);
        //    }
        //    else if (controller.GetAxis(Valve.VR.EVRButtonId.k_EButton_SteamVR_Touchpad).y < -0.5)
        //    {
        //        Debug.Log("Dpad Down");
        //        scaleDown(selected);
        //        selected.transform.localScale -= new Vector3(scaleFactor, scaleFactor, scaleFactor);
        //    }
        //}


    }

	void OnTriggerEnter(Collider collider) {
		selected = collider.gameObject;
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
        Vector3 controllerRightPosition = controllerRight.transform.pos;
        Vector3 controllerLeftPosition = controllerLeft.transform.pos;
        float distance = Vector3.Distance(controllerLeftPosition, controllerRightPosition);
        if(oldControllerDistance != float.NaN)
        {
            float delta = distance - oldControllerDistance;
            selected.transform.localScale = Vector3.Lerp(selected.transform.localScale, Vector3.one * delta, Time.deltaTime);
        }

        oldControllerDistance = distance;
    }
}
