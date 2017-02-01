﻿using UnityEngine;
using System.Collections;

public class ScaleController : MonoBehaviour {

	private SteamVR_TrackedObject trackedObj;
	private SteamVR_Controller.Device controller;
	private Valve.VR.EVRButtonId gripButton = Valve.VR.EVRButtonId.k_EButton_Grip;
	private GameObject selected;
	private float diff;

	// Use this for initialization
	void Start () {
		trackedObj = GetComponent<SteamVR_TrackedObject>();
		controller = SteamVR_Controller.Input((int)trackedObj.index);
	}
	
	// Update is called once per frame
	void Update () {
		if(controller.GetPress(gripButton)){
			while(controller.GetPressUp(gripButton) == false)
			{
				diff = Vector3.Dot(controller.velocity, selected.transform.position);
				controller.TriggerHapticPulse(500, gripButton);
				scale();
			}
		}
	}

	public void scale(){
		if(diff >= 0 && selected.transform.localScale.magnitude > new Vector3(0.2F, 0.2F, 0.2F).magnitude){
			selected.transform.localScale -= new Vector3(0.1F, 0.1F, 0.1F);
		}
		if(diff < 0){
			selected.transform.localScale += new Vector3(0.1F, 0.1F, 0.1F);
		}
	}

    public void OnTriggerEnter(Collider collider)
    {
        selected = collider.gameObject;
        if (GripIsPressed())
        {
            Debug.Log("Here");
            if (controller.GetPressUp(gripButton) == false)
            {
                diff = controller.velocity.magnitude;
                //controller.TriggerHapticPulse (500, gripButton);
                scale();
            }
        }
    }

    public bool GripIsPressed(){
		if (controller != null) {
			return controller.GetPress (gripButton);
		}

		return false;
		//return controller.GetPress (gripButton);
	}

}