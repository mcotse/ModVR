using UnityEngine;
using System.Collections;

public class ScaleController : MonoBehaviour {

	private SteamVR_TrackedObject trackedObj;
	private SteamVR_Controller.Device controller;
	private Valve.VR.EVRButtonId gripButton = Valve.VR.EVRButtonId.k_EButton_Grip;

	private GameObject pickup;
	// Use this for initialization
	void Start () {
		trackedObj = GetComponent<SteamVR_TrackedObject>();
		controller = SteamVR_Controller.Input((int)trackedObj.index);
	}
	
	// Update is called once per frame
	void Update () {
		GameObject obj = gameObject.GetComponent<SphereCollider>().gameObject;
		if(obj != null){
			if(controller.GetPressDown(gripButton)){
				Debug.Log("Grip Pressed");
				Vector3 ctrlrVelocity = controller.velocity;
				Vector3 objPos = obj.transform.position;
				float diff = Vector3.Dot(ctrlrVelocity, objPos);
				if(diff >= 0){

					obj.transform.localScale += new Vector3(diff, diff, diff);
				}
				else{
					obj.transform.localScale -= new Vector3(diff, diff, diff);
				}

				controller.TriggerHapticPulse(500, gripButton);


				//Transform selected Object
			}
		}

		return;
	}
}
