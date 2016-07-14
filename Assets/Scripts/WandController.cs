using UnityEngine;
using System.Collections;

public class WandController : MonoBehaviour {

	public GameObject menu;

	private SteamVR_TrackedObject trackedObj;
	private Valve.VR.EVRButtonId menuButton = Valve.VR.EVRButtonId.k_EButton_ApplicationMenu;

	private bool menuButtonDown = false;
	private bool menuButtonUp = false;

	// Use this for initialization
	void Start () {
		trackedObj = GetComponent<SteamVR_TrackedObject> ();
		menu.SetActive (false);
	}

	// Update is called once per frame
	void Update () {
		SteamVR_Controller.Device controller = SteamVR_Controller.Input ((int)trackedObj.index);

		if (controller == null) {
			Debug.Log ("Controller not initialized");
			return;
		}

		menuButtonDown = controller.GetPressDown (menuButton);
		menuButtonUp = controller.GetPressUp (menuButton);

		if (menuButtonDown) {
			Debug.Log ("Menu Button pressed..");
			menu.SetActive (true);
		}
		if (menuButtonUp) {
			Debug.Log ("Menu Button released..");
		}
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
