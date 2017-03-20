using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using VRTK;
using VRTK.SecondaryControllerGrabActions;
using VRTK.GrabAttachMechanics;
using VRTK.Highlighters;
using ModVR;

public class ModVR_WandController : MonoBehaviour {

    public GameObject menu;

    private GroupUtil util;
    private GameObject otherController;
    private VRTK_ControllerActions actions;
    private VRTK_ControllerEvents events;

    private uint indexMain;
    private uint indexSecondary;

    private bool isSelectMode;
    private bool isInteractMode;
    private bool menuButtonDown;
    private bool showMenu;
    private bool triggerPressed;
    private bool gripPressed;

    private GameObject selected;
    private GameObject grabbed;
    Color color = new Color(237, 241, 39);

    private UnityEngine.Events.UnityEvent OnGroup;

    // Use this for initialization
    void Start () {
        menuButtonDown = false;
        isSelectMode = false;
        isInteractMode = true;
        showMenu = false;
        gripPressed = false;
        menu.SetActive(false);
        util = new GroupUtil();

        actions = GetComponent<VRTK_ControllerActions>();
        events = GetComponent<VRTK_ControllerEvents>();

        events.ButtonOnePressed += OnMenuButtonPressed;
        events.TouchpadPressed += OnTouchpadPressed;
        events.TriggerPressed += OnTriggerPressed;
		events.GripPressed += OnGripPressed;
        events.TouchpadTouchStart += OnTouchpadTouched;
        // events.GripPressed += GroupOnPressed;
        //events.GripPressed += MergeOnPressed;

        GameObject objectOptions = gameObject.transform.Find("ObjectOptions").gameObject;
        GameObject radialMenu = gameObject.transform.Find("RadialMenu").gameObject;

        if(objectOptions && radialMenu)
        {
            GameManager.instance.objectOptions = objectOptions;
            GameManager.instance.radialMenu = radialMenu;
        }
	}


    // Update is called once per frame
    void Update()
    {

    }


    private void ToggleMenu()
    {
        showMenu = !showMenu;
        menu.SetActive(showMenu);
    }

    private void SetupInteractableObject(GameObject obj)
    {
        Rigidbody rb = obj.GetComponent<Rigidbody>();
        if (rb == null)
        {
            rb = obj.AddComponent<Rigidbody>();
        }
        
        rb.freezeRotation = false;
        rb.detectCollisions = true;
        rb.collisionDetectionMode = CollisionDetectionMode.Discrete;
        rb.isKinematic = true;
        rb.useGravity = false;

        if (obj.GetComponent<MeshCollider>() == null && obj.GetComponent<BoxCollider>() == null && (obj.name.StartsWith("merged") || obj.name.StartsWith("groupObj")))
        {
            BoxCollider bc = obj.AddComponent<BoxCollider>();
            Bounds bcBounds = new Bounds();

            foreach (Transform t in obj.transform)
            {
                if (!t.name.Contains("Highlight"))
                {
                    Bounds colBounds = t.GetComponent<Collider>().bounds;
                    if(bcBounds.extents == Vector3.zero)
                    {
                        bcBounds = colBounds;
                    }
                    bcBounds.Encapsulate(colBounds);
                }
            }
            bc.center = bcBounds.center;
            bc.size = bcBounds.size;
            bc.isTrigger = true;
        }

        ModVR_InteractableObject io = obj.GetComponent<ModVR_InteractableObject>();
        if(io == null)
        {
            io = obj.AddComponent<ModVR_InteractableObject>();
        }
        io.isUsable = true;
        io.touchHighlightColor = Color.red;
        io.pointerActivatesUseAction = false;
        io.enabled = true;
        io.isGrabbable = true;
        io.holdButtonToGrab = true;

        VRTK_ChildOfControllerGrabAttach grabAttach = obj.AddComponent<VRTK_ChildOfControllerGrabAttach>();
        grabAttach.precisionGrab = true;
        io.grabAttachMechanicScript = grabAttach;

        io.secondaryGrabActionScript = obj.AddComponent<VRTK_AxisScaleGrabAction>();

        GameManager.instance.AddInteractableObject(io);
        //obj.AddComponent<VRTK_FixedJointGrabAttach>();

        ModVR_OutlineObjectSelectHighlighter selectHighlighter = obj.AddComponent<ModVR_OutlineObjectSelectHighlighter>();
        
        selectHighlighter.Initialise(color);

        VRTK_OutlineObjectCopyHighlighter highligher = obj.GetComponent<VRTK_OutlineObjectCopyHighlighter>();
        if(highligher == null)
        {
            highligher = obj.AddComponent<VRTK_OutlineObjectCopyHighlighter>();
        }

        
    }

    void CreateSelectedObject(ModVR_InteractableObject selectedObj)
    {
        GameObject selected = selectedObj.gameObject;
        GameObject newGameObj = Instantiate(selected, selected.transform.position, selected.transform.rotation);
        Guid gameObjName = Guid.NewGuid();
        newGameObj.name = gameObjName.ToString() + "_" + newGameObj.name;
        SetupInteractableObject(newGameObj);
    }

	public void GetNewRadialMenuOptions()
	{
		GameObject radialMenu = GameObject.Find ("RadialMenu");
		if (radialMenu != null) {
			radialMenu.SetActive (false);
			//GameObject optionsMenu = gameObject.FindObject ("Options");
			GameObject optionsMenu = null;
			Component[] components = transform.GetComponentsInChildren(typeof(Transform), true);
			foreach(Component c in components){
				if(c.gameObject.name == "Options"){
					optionsMenu = c.gameObject;
				}
			}
			if (optionsMenu != null) {
				optionsMenu.SetActive (true);
			} else {
				Debug.Log ("optionsMenu is null");
			}
		}
	}


    #region Controller Events
    private void OnMenuButtonPressed(object sender, ControllerInteractionEventArgs e)
    {
        indexMain = e.controllerIndex;
        menuButtonDown = !menuButtonDown;

        if (isInteractMode)
        {
            ToggleMenu();
        }
    }

    private void OnTouchpadPressed(object sender, ControllerInteractionEventArgs e)
    {
        isSelectMode = !isSelectMode;
        isInteractMode = !isInteractMode;

        if (isSelectMode)
        {

            if (showMenu)
            {
                ToggleMenu();
            }
        }
    }

    private void OnGripPressed(object sender, ControllerInteractionEventArgs e)
    {
        if (isInteractMode)
        {
            ModVR_InteractableObject selectedObj = (from io in GameObject.FindObjectsOfType<ModVR_InteractableObject>()
                                                    where io.IsTouched() && io.GetTouchingObjects().Contains(this.gameObject)
                                                    select io).SingleOrDefault();


            if (selectedObj != null && selectedObj.transform.parent != null)
            {
                string parentName = selectedObj.transform.parent.name;
                if (parentName.Equals("MenuRight") || parentName.Equals("MenuLeft"))
                {
                    CreateSelectedObject(selectedObj);
                }
            }
        }
    }
    private void OnTriggerPressed(object sender, ControllerInteractionEventArgs e)
    {
        GameObject triggeredObj = sender as GameObject;
        if (isSelectMode)
        {
            if (GameManager.instance.laserColliding)
            {
                GameObject go = GameManager.instance.lastLaserSelectedObj;
                ModVR_OutlineObjectSelectHighlighter selector = go.GetComponent<ModVR_OutlineObjectSelectHighlighter>();

                bool isSelected = GameManager.instance.handleSelectedObject(go);


                if (isSelected == true)
                {
                    selector.Highlight(color);
                }
                else
                {
                    selector.Unhighlight(Color.clear);
                }


            }
        }
    }

    private void OnTouchpadTouched(object sender, ControllerInteractionEventArgs e)
    {
        if (GameManager.instance.selectedObjectList.Count > 0)
        {
            GameManager.instance.objectOptions.SetActive(true);
            GameManager.instance.radialMenu.SetActive(false);
        }
        else
        {
            GameManager.instance.objectOptions.SetActive(false);
            GameManager.instance.radialMenu.SetActive(true);
        }
    }

    #endregion


    #region Radial Menu Events

    //Object Options Radial Menu
    public void OnMergeClick()
    {
        List<ModVR_InteractableObject> objList = GameManager.instance.interactableObjectList;
        List<List<string>> collisionSet = GameManager.instance.collisionSet;
        

        GameObject merged = util.mergeGroups(objList, collisionSet);
        
        foreach(Transform t in merged.transform)
        {
            SetupInteractableObject(t.gameObject);
        }

        GameManager.instance.selectedObjectList = new List<ModVR_InteractableObject>();
        GameManager.instance.collisionSet = new List<List<string>>();

        foreach(List<string> collision in collisionSet)
        {
            GameManager.instance.interactableObjectList = GameManager.instance.interactableObjectList.Where(o => o.name != collision[0]).ToList();
            GameManager.instance.interactableObjectList = GameManager.instance.interactableObjectList.Where(o => o.name != collision[1]).ToList();
        }
    }

    public void OnDeleteClicked()
    {
        List<ModVR_InteractableObject> selectedObjs = GameManager.instance.selectedObjectList;
        List<ModVR_InteractableObject> itemsToRemove = GameManager.instance.interactableObjectList.Union(selectedObjs).ToList();
        List<string> objNames = selectedObjs.Select(d => d.name).ToList();

        foreach(ModVR_InteractableObject io in itemsToRemove)
        {
            GameManager.instance.RemoveInteractableObject(io);
        }

        foreach (string name in objNames)
        {
            GameManager.instance.RemoveCollisionByName(name);
        }

        foreach (ModVR_InteractableObject io in selectedObjs)
        {
            Destroy(io.gameObject);
        }

        GameManager.instance.selectedObjectList = new List<ModVR_InteractableObject>();
        
    }
    

    public void OnUngroupClicked()
    {
        List<ModVR_InteractableObject> grouped = GameManager.instance.selectedObjectList;
        
        List<GameObject> objsToRemove = new List<GameObject>();
        foreach(ModVR_InteractableObject io in grouped)
        {
            foreach (Transform t in io.transform)
            {
                util.unGroupObject(t.gameObject);
                ModVR_OutlineObjectSelectHighlighter highlighter = t.gameObject.GetComponent<ModVR_OutlineObjectSelectHighlighter>();
                if (highlighter == null)
                {
                    highlighter = t.gameObject.AddComponent<ModVR_OutlineObjectSelectHighlighter>();
                    
                }

                highlighter.Initialise(color);
            }
            io.transform.DetachChildren();
            objsToRemove.Add(io.gameObject);
            GameManager.instance.RemoveInteractableObject(io);
        }

        foreach(GameObject go in objsToRemove)
        { 
            Destroy(go);
        }

        GameManager.instance.selectedObjectList = new List<ModVR_InteractableObject>();
    }

    public void OnGroupClicked(string message)
    {
        if (GameManager.instance.selectedObjectList.Count > 1)
        {
            GameObject grouped = util.groupObjects(GameManager.instance.selectedObjectList);
            SetupInteractableObject(grouped);
            GameManager.instance.selectedObjectList = new List<ModVR_InteractableObject>();
        }
    }

    public void OnExportClicked()
    {
        List<ModVR_InteractableObject> selected = GameManager.instance.selectedObjectList;
        foreach(ModVR_InteractableObject io in selected)
        {
            ModVR_ObjExporter.GameObjectToFile(io.gameObject);
            io.GetComponent<ModVR_OutlineObjectSelectHighlighter>().Unhighlight();

            GameManager.instance.UpdateLastSaved(io.name);
        }



        GameManager.instance.selectedObjectList = new List<ModVR_InteractableObject>();
    }
    public void OnImportClicked()
    {
        
        GameObject go = ModVR_ObjImporter.ImportLastSavedObject(GameManager.instance.lastSaved);
        
        go.AddComponent<BoxCollider>();
        SetupInteractableObject(go);
        go.transform.position = gameObject.transform.position;

    }

    #endregion
}
