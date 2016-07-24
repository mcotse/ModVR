using UnityEngine;
using System.Collections;

public class ScaleController : MonoBehaviour {

	private SteamVR_TrackedObject trackedObj;
	private SteamVR_Controller.Device controller;
	//private Valve.VR.EVRButtonId triggerButton = Valve.VR.EVRButtonId.k_EButton_SteamVR_Trigger;

	private GameObject pickup;
	// Use this for initialization
	void Start () {
		trackedObj = GetComponent<SteamVR_TrackedObject>();
		controller = SteamVR_Controller.Input((int)trackedObj.index);
	}
	
	// Update is called once per frame
	void Update () {
		Debug.Log("In update");
		SphereCollider s = gameObject.GetComponent<SphereCollider>();
		if(s != null){
			Debug.Log("Sphere object found");
			if(controller.GetHairTriggerDown()){
				Debug.Log("Trigger Pressed");
				s.radius += 1;
				//Transform selected Object
			}
		}

		return;
	}
}
