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

	private bool menuButtonDown;
	//private bool triggerButtonDown;
	//private bool triggerButtonUp;
	private bool showMenu;

	private GameObject selected;

	private Gizmo gizmoControl;
	// Use this for initialization
	void Start () {
		trackedObj = GetComponent<SteamVR_TrackedObject> ();
		menu.SetActive (false);
		menuButtonDown = false;
		showMenu = false;

		gizmoControl = GameObject.Find("Gizmo").GetComponent<Gizmo>();
	}

	// Update is called once per frame
	void Update () {
		SteamVR_Controller.Device controller = SteamVR_Controller.Input ((int)trackedObj.index);

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
			selected.transform.SetParent (this.transform);
			selected.GetComponent<Rigidbody> ().isKinematic = true;
		}
		if (controller.GetPressUp (triggerButton) && selected != null) {
			selected.transform.SetParent (null);
			selected.GetComponent<Rigidbody> ().isKinematic = false;
		}

		if(controller.GetPressDown(gripButton) && selected != null){
			Debug.Log ("Grip Button pressed!");

			//gizmoControl.Show();
            //gizmoControl.SelectObject(selected.transform);
			//gameObject.layer = 2;
		}

		if(controller.GetPressUp(gripButton)){
			gameObject.layer = 0;
			gizmoControl.ClearSelection();
		}

		if (controller.GetPressDown (SteamVR_Controller.ButtonMask.Touchpad) && selected != null) {
			//Debug.Log ("Touchpad pressed");
			if (controller.GetAxis (Valve.VR.EVRButtonId.k_EButton_SteamVR_Touchpad).y > 0.5f) {
				Debug.Log ("Dpad Up");
				selected.transform.localScale += new Vector3 (scaleFactor, scaleFactor, scaleFactor);
			} else if (controller.GetAxis (Valve.VR.EVRButtonId.k_EButton_SteamVR_Touchpad).y < -0.5) {	
				Debug.Log ("Dpad Down");
				selected.transform.localScale -= new Vector3 (scaleFactor, scaleFactor, scaleFactor);
			}
		}
	}

	void OnTriggerEnter(Collider collider) {
		selected = collider.gameObject;
	}

	void OnTriggerExit(Collider collider) {
		selected = null;
	}
}
