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
        events.TriggerClicked += OnTriggerClicked;
        // events.GripPressed += GroupOnPressed;
        events.GripPressed += MergeOnPressed;
	}


    // Update is called once per frame
    void Update()
    {

    }


    private void OnMenuButtonPressed(object sender, ControllerInteractionEventArgs e)
    {
        indexMain = e.controllerIndex;
        menuButtonDown = !menuButtonDown;

        if (isInteractMode)
        {
            ToggleMenu();
        }
    }

    private void GroupOnPressed(object sender, ControllerInteractionEventArgs e)
    {
        util.groupObjects(GameManager.instance.interactableObjectList);
    }

    private void MergeOnPressed(object sender, ControllerInteractionEventArgs e)
    {
        List<ModVR_InteractableObject> objList = GameManager.instance.interactableObjectList;
        List<List<string>> collisionSet = GameManager.instance.collisionSet;
        GameObject merged = util.mergeGroups(objList, collisionSet);
        SetupInteractableObject(merged);
    }

    private void OnTouchpadPressed(object sender, ControllerInteractionEventArgs e)
    {
        isSelectMode = !isSelectMode;
        isInteractMode = !isInteractMode;

        if(isSelectMode && showMenu)
        {
            ToggleMenu();
        }
    }

    private void OnTriggerClicked(object sender, ControllerInteractionEventArgs e)
    {
        GameObject triggeredObj = sender as GameObject;
        if (isInteractMode)
        {
            ModVR_InteractableObject selectedObj = (from io in GameObject.FindObjectsOfType<ModVR_InteractableObject>()
                                            where io.IsTouched() && io.GetTouchingObjects().Contains(triggeredObj)
                                            select io).SingleOrDefault();
            //GameObject ioObj = null;

            //ModVR_InteractableObject[] objects = FindObjectsOfType<ModVR_InteractableObject>();
            //foreach(ModVR_InteractableObject obj in objects)
            //{
            //    bool touched = obj.IsTouched();
            //    List<GameObject> touchingObjs = obj.GetTouchingObjects();
            //    ioObj = touchingObjs.Where(go => go.name == triggeredObj.transform.name).SingleOrDefault();
            //}

            string parentName = selectedObj.transform.parent.name;
            if (selectedObj && (parentName.Equals("MenuRight") || parentName.Equals("MenuLeft")))
            {
                CreateSelectedObject(selectedObj);
            }
        }
        if (isSelectMode){
            if (GameManager.instance.laserColliding)
            {
                ModVR_OutlineObjectSelectHighlighter selector = GameManager.instance.lastLaserSelectedObj.GetComponent<ModVR_OutlineObjectSelectHighlighter>();

                bool isSelected = GameManager.instance.handleSelectedObject(GameManager.instance.lastLaserSelectedObj);

                
                if(isSelected == true)
                {
                    selector.Highlight(Color.blue);
                }
                else
                {
                    selector.ResetHighlighter();
                }

            }
        }
    }


    private void ToggleMenu()
    {
        showMenu = !showMenu;
        menu.SetActive(showMenu);
    }

    private void SetupInteractableObject(GameObject obj)
    {
        if (obj.GetComponent<Rigidbody>() == null)
        {
            Rigidbody rb = obj.AddComponent<Rigidbody>();
            rb.freezeRotation = false;
            rb.detectCollisions = true;
            rb.collisionDetectionMode = CollisionDetectionMode.Discrete;
            rb.isKinematic = false;
            rb.useGravity = false;
        }

        if(obj.GetComponent<BoxCollider>() == null && obj.name.StartsWith("merged"))
        {
            BoxCollider bc = obj.AddComponent<BoxCollider>();
            bc.isTrigger = true;
        }

        ModVR_InteractableObject io = obj.GetComponent<ModVR_InteractableObject>();
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

        Rigidbody rigidObj = obj.GetComponent<Rigidbody>();
        rigidObj.constraints = RigidbodyConstraints.None;
        rigidObj.useGravity = false;
        rigidObj.isKinematic = false;

        GameManager.instance.AddInteractableObject(io);
        obj.AddComponent<VRTK_FixedJointGrabAttach>();

        ModVR_OutlineObjectSelectHighlighter selectHighlighter = obj.AddComponent<ModVR_OutlineObjectSelectHighlighter>();
        selectHighlighter.Initialise(Color.blue);

        obj.AddComponent<VRTK_OutlineObjectCopyHighlighter>();
    }

    void CreateSelectedObject(ModVR_InteractableObject selectedObj)
    {
        GameObject selected = selectedObj.gameObject;
        GameObject newGameObj = Instantiate(selected, selected.transform.position, selected.transform.rotation);
        Guid gameObjName = Guid.NewGuid();
        newGameObj.name = gameObjName.ToString() + "_" + newGameObj.name;
        SetupInteractableObject(newGameObj);
    }
}
