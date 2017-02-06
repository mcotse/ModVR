using UnityEngine;
using System.Collections.Generic;
using VRTK;

public class WandController : MonoBehaviour {

	public GameObject menu;
	public GameObject cube;
	public float scaleFactor;
	public GroupUtil GroupUtil;
	private SteamVR_TrackedObject trackedObj;
	private Valve.VR.EVRButtonId gripButton = Valve.VR.EVRButtonId.k_EButton_Grip;
	private Valve.VR.EVRButtonId menuButton = Valve.VR.EVRButtonId.k_EButton_ApplicationMenu;
	private Valve.VR.EVRButtonId triggerButton = Valve.VR.EVRButtonId.k_EButton_SteamVR_Trigger;
	private Valve.VR.EVRButtonId touchPadUp = Valve.VR.EVRButtonId.k_EButton_DPad_Up;
    private Valve.VR.EVRButtonId touchPadLeft = Valve.VR.EVRButtonId.k_EButton_DPad_Left;
    private Valve.VR.EVRButtonId touchPadRight = Valve.VR.EVRButtonId.k_EButton_DPad_Right;
    private Valve.VR.EVRButtonId touchPadown = Valve.VR.EVRButtonId.k_EButton_DPad_Down;

    SteamVR_Controller.Device controllerMain;
    SteamVR_Controller.Device controllerSecondary;

    private bool menuButtonDown;
	private bool showMenu;
    private bool gripIsPressed;
    private bool bothTriggersPressed;
    private bool isSelectMode;

	private GameObject selected;
	private GameObject grabbed;

    private HashSet<List<string>> collisions = new HashSet<List<string>>();

	// Use this for initialization
	void Start () {
		trackedObj = GetComponent<SteamVR_TrackedObject> ();
		menu.SetActive (false);
		menuButtonDown = false;
		showMenu = false;

        setupControllers();
    }

	// Update is called once per frame
	void Update () {
		//SteamVR_Controller.Device controller = SteamVR_Controller.Input ((int)trackedObj.index);

		if (controllerMain == null) {
			Debug.Log ("Controller not initialized");
			return;
		}


        //menu
		menuButtonDown = controllerMain.GetPressDown (menuButton);

		if (menuButtonDown) {
			showMenu = !showMenu;
			menu.SetActive (showMenu);
		}


        //grabbing
		if (controllerMain.GetPressDown (triggerButton) && selected != null) {
			if (selected.transform.parent != null && (selected.transform.parent.name).StartsWith ("Menu")) {
				GameObject newGameObj = Instantiate (selected, selected.transform.position, selected.transform.rotation);
				grabbed = newGameObj;
			} else {
				grabbed = selected;
			}
			grabbed.transform.SetParent (this.transform);
			grabbed.GetComponent<Rigidbody> ().isKinematic = true;
		}
		if (controllerMain.GetPressUp (triggerButton) && grabbed != null) {
			grabbed.transform.SetParent (null);
			grabbed.GetComponent<Rigidbody> ().isKinematic = false;
			grabbed = null;
		}


        //single axis scaling
        if (controllerSecondary.GetPressDown(Valve.VR.EVRButtonId.k_EButton_Grip))
        {
            gripIsPressed = true;
        }
        if (controllerSecondary.GetPressUp(Valve.VR.EVRButtonId.k_EButton_Grip))
        {
            gripIsPressed = false;
        }

        if (gripIsPressed && selected != null)
        {
            scaleSelected(selected);
        }


        //Destroy objects
        if (selected != null && controllerMain.GetPressDown(gripButton))
        {
            Object.Destroy(selected.gameObject);
        }



        //enlarge selected
        if (controllerSecondary.GetPressDown(triggerButton))
        {
            bothTriggersPressed = true;
        }
        if (controllerSecondary.GetPressUp(triggerButton))
        {
            bothTriggersPressed = false;
        }

        if (bothTriggersPressed && selected != null)
        {
            enlargeSelected(selected);
        }


        if (controllerMain.GetPressDown(touchPadUp) && isSelectMode == false)
        {
            isSelectMode = true;
        }

        if (controllerMain.GetPressDown(touchPadUp) && isSelectMode)
        {
            isSelectMode = false;
            //clearSelection();
            collisions = new HashSet<List<string>>();
        }

        if (controllerMain.GetPressDown(touchPadLeft) && isSelectMode)
        {
            // call Matts code
						GroupUtil = new GroupUtil();
            List<GameObject> selectedObjs = getSelection();
            // merge(selectedObjs, collisions);
						GroupUtil.mergeGroups(selectedObjs,collisions);
        }

    }

    void OnTriggerEnter(Collider collider) {
		selected = collider.gameObject;
		setupControllers ();
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

    void scaleSelected(GameObject selected)
    {
        Vector3 velocity = controllerSecondary.velocity;
        Vector3 newScale = Vector3.zero;
        float scalingFactor = 2.0f;

        float max = Mathf.Max(Mathf.Max(Mathf.Abs(velocity.x), Mathf.Abs(velocity.y)), Mathf.Abs(velocity.z));

        if(max == Mathf.Abs(velocity.x))
        {
            newScale = selected.transform.localScale + new Vector3(velocity.x * scalingFactor, 0, 0);
        }
        else if(max == Mathf.Abs(velocity.y))
        {
            newScale = selected.transform.localScale + new Vector3(0, velocity.y * scalingFactor, 0);
        }
        else if (max == Mathf.Abs(velocity.z))
        {
            newScale = selected.transform.localScale + new Vector3(0, 0, velocity.z * scalingFactor);
        }

        selected.transform.localScale = Vector3.Lerp(selected.transform.localScale, newScale, Time.deltaTime);
    }

    void enlargeSelected(GameObject selected)
    {
        Vector3 velocity = controllerSecondary.velocity;

        float scale = 2.0f;
        if (controllerSecondary.index == SteamVR_Controller.GetDeviceIndex(SteamVR_Controller.DeviceRelation.Leftmost))
        {
            scale = -1 * scale;
        }
        Vector3 newScale = Vector3.zero;


        newScale = selected.transform.localScale + new Vector3(velocity.x * scale, velocity.x * scale, velocity.x * scale);

        selected.transform.localScale = Vector3.Lerp(selected.transform.localScale, newScale, Time.deltaTime);
    }

    void setupControllers()
    {
        if ((int)trackedObj.index == SteamVR_Controller.GetDeviceIndex(SteamVR_Controller.DeviceRelation.Leftmost))
        {
            controllerMain = SteamVR_Controller.Input((int)trackedObj.index);
            controllerSecondary = SteamVR_Controller.Input(SteamVR_Controller.GetDeviceIndex(SteamVR_Controller.DeviceRelation.Rightmost));
        }
        else
        {
            controllerMain = SteamVR_Controller.Input((int)trackedObj.index);
            controllerSecondary = SteamVR_Controller.Input(SteamVR_Controller.GetDeviceIndex(SteamVR_Controller.DeviceRelation.Leftmost));

        }
    }

    void OnCollisionStayEvent(List<string> names)
    {
        if (isSelectMode)
        {
            collisions.Add(names);
        }
    }

    void clearSelection()
    {
        ObjectEvents[] oe = FindObjectsOfType<ObjectEvents>();
        foreach (ObjectEvents obj in oe)
        {
            if (obj.isSelected)
            {
                obj.isSelected = false;
            }
        }
    }

    List<GameObject> getSelection()
    {
        List<GameObject> goList = new List<GameObject>();

        ObjectEvents[] oe = FindObjectsOfType<ObjectEvents>();
        foreach (ObjectEvents obj in oe)
        {
            if (obj.isSelected)
            {
                goList.Add(obj.gameObject);
            }
        }

        return goList;
    }
}
