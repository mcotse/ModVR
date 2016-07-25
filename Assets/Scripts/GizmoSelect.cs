using UnityEngine;
using System.Collections;

public class GizmoSelect : MonoBehaviour 
{
    private Gizmo gizmoControl;
    private bool shiftDown;
    private SteamVR_TrackedObject trackedObj;
	private Valve.VR.EVRButtonId gripButton = Valve.VR.EVRButtonId.k_EButton_Grip;
	private SteamVR_Controller.Device controller;
	// Use this for initialization
	void Start () 
    {
        trackedObj = gameObject.GetComponent<SteamVR_TrackedObject> ();
        gizmoControl = GameObject.Find("Gizmo").GetComponent<Gizmo>();
        
	}
	
    void OnTriggerEnter(Collider collider)
    {
		//trackedObj = collider.gameObject;
    }
	// Update is called once per frame
	/*
	void Update () 
    {
		controller = SteamVR_Controller.Input ((int)trackedObj.index);
		if (controller.GetPressDown (gripButton)) {
			Debug.Log ("Grip button!");
			if (gizmoControl != null) {
				if (!shiftDown) {
					gizmoControl.ClearSelection ();
				}
				gizmoControl.Show ();
				gizmoControl.SelectObject (transform);
				gameObject.layer = 2;
				shiftDown = false;
			}
		}
		if (!controller.GetPressUp(gripButton) ){
			shiftDown = true;
		}
	}
	*/

    public void Unselect()
    {
        gameObject.layer = 0;
    }
}
