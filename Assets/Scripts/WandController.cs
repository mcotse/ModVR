using UnityEngine;
using System.Collections;

public class WandController : MonoBehaviour {

	public GameObject menu;

	private SteamVR_TrackedObject trackedObj;
	private Valve.VR.EVRButtonId menuButton = Valve.VR.EVRButtonId.k_EButton_ApplicationMenu;
	private Valve.VR.EVRButtonId triggerButton = Valve.VR.EVRButtonId.k_EButton_SteamVR_Trigger;

	private bool menuButtonDown;
	//private bool triggerButtonDown;
	//private bool triggerButtonUp;
	private bool showMenu;

	private GameObject selected;

	// Use this for initialization
	void Start () {
		trackedObj = GetComponent<SteamVR_TrackedObject> ();
		menu.SetActive (false);
		menuButtonDown = false;
		showMenu = false;
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
			Debug.Log ("1st case");
			selected.transform.parent = this.transform;
			selected.GetComponent<Rigidbody> ().isKinematic = true;
		}
		if (controller.GetPressUp (triggerButton) && selected != null) {
			Debug.Log ("2nd case");
			selected.transform.parent = null;
			selected.GetComponent<Rigidbody> ().isKinematic = false;
		}
	}

	void OnTriggerEnter(Collider collider) {
		selected = collider.gameObject;
	}

	void OnTriggerExit(Collider collider) {
		selected = null;
	}

	/*
	private SteamVR_Controller.Device controller {
		get {
			return (SteamVR_Controller.Input ((int)trackedObj.index));
		}
	}
		
	private SteamVR_TrackedObject trackedObj;
	private Valve.VR.EVRButtonId menuButton = Valve.VR.EVRButtonId.k_EButton_ApplicationMenu;

	public bool menuButtonDown = false;
	public bool menuButtonUp = false;

	// Use this for initialization
	void Start () {
		trackedObj = GetComponent<SteamVR_TrackedObject> ();
	}
	
	// Update is called once per frame
	void Update () {
		if (controller == null) {
			Debug.Log ("Controller not initialized");
			return;
		}

		menuButtonDown = controller.GetPressDown (menuButton);
		menuButtonUp = controller.GetPressUp (menuButton);

		if (menuButtonDown) {
			Debug.Log ("Menu Button pressed..");
		}
		if (menuButtonUp) {
			Debug.Log ("Menu Button released..");
		}
	}
	*/
}
