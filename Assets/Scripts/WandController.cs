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

	private bool menuButtonDown;
	private bool showMenu;

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

		if (controller.GetPressDown (SteamVR_Controller.ButtonMask.Touchpad) && selected != null) {
			//Debug.Log ("Touchpad pressed");
			if (controller.GetAxis (Valve.VR.EVRButtonId.k_EButton_SteamVR_Touchpad).y > 0.5f) {
				Debug.Log ("Dpad Up");
                scaleUp(selected);
				//selected.transform.localScale += new Vector3 (scaleFactor, scaleFactor, scaleFactor);
			} else if (controller.GetAxis (Valve.VR.EVRButtonId.k_EButton_SteamVR_Touchpad).y < -0.5) {	
				Debug.Log ("Dpad Down");
                scaleDown(selected);
				//selected.transform.localScale -= new Vector3 (scaleFactor, scaleFactor, scaleFactor);
			}
		}
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

    public void scaleUp(GameObject selected)
    {
        selected.transform.localScale = Vector3.Lerp(selected.transform.localScale, Vector3.one*scaleFactor, Time.deltaTime);
    }

    public void scaleDown(GameObject selected)
    {
        selected.transform.localScale = Vector3.Lerp(selected.transform.localScale, Vector3.one * scaleFactor* -1, Time.deltaTime);
    }
}
