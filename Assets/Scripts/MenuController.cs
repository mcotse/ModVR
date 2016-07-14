using UnityEngine;
using System.Collections;

public class MenuController : MonoBehaviour {

	//public Renderer renderer;

	private SteamVR_TrackedObject trackedObj;
	private Valve.VR.EVRButtonId menuButton = Valve.VR.EVRButtonId.k_EButton_ApplicationMenu;

	private bool menuButtonDown = false;
	private bool menuButtonUp = false;

	// Use this for initialization
	void Start () {
		trackedObj = GetComponent<SteamVR_TrackedObject> ();
		//GetComponent<Renderer>().enabled = false;
		gameObject.SetActive (false);
	}
	/*
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
			ToggleVisibility ();
		}
		if (menuButtonUp) {
			Debug.Log ("Menu Button released..");
		}
	}

	void ToggleVisibility() {
		Component [] components = gameObject.GetComponentsInChildren<Renderer> ();
		foreach (Renderer r in gameObject.GetComponentsInChildren<Renderer> ()) {
			r.enabled = !r.enabled;
		}
	}
	*/
}
