using UnityEngine;
using System.Collections;

public class GizmoSelect : MonoBehaviour 
{
    private Gizmo gizmoControl;
    private bool shiftDown;
    private SteamVR_TrackedObject trackedObj;
	// Use this for initialization
	void Start () 
    {
        trackedObj = GetComponent<SteamVR_TrackedObject> ();
        gizmoControl = GameObject.Find("Gizmo").GetComponent<Gizmo>();
        
	}
	
    void OnTriggerEnter(Collider collider)
    {
        SteamVR_Controller.Device controller = SteamVR_Controller.Input ((int)trackedObj.index);
        if(controller.GetTouchDown((int)Valve.VR.EVRButtonId.k_EButton_SteamVR_Touchpad))
        {
            if (gizmoControl != null)
            {
                if (!shiftDown)
                {
                    gizmoControl.ClearSelection();
                }
                gizmoControl.Show();
                gizmoControl.SelectObject(transform);
                gameObject.layer = 2;
            }
        }
    }
	// Update is called once per frame
	void Update () 
    {
        if (Input.GetKey(KeyCode.LeftShift) || Input.GetKeyDown(KeyCode.RightShift))
        {
            shiftDown = true;
        }
        else
        {
            shiftDown = false;
        }
	}

    public void Unselect()
    {
        gameObject.layer = 0;
    }
}
