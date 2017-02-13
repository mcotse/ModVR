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
        List<VRTK_InteractableObject> objList = GameManager.instance.interactableObjectList;
        List<List<string>> collisionSet = GameManager.instance.collisionSet;
        util.mergeGroups(objList, collisionSet);
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
        if (isInteractMode)
        {
            ObjectEvents selectedObj = (from io in GameObject.FindObjectsOfType<ObjectEvents>()
                                            where io.IsTouched() && io.GetTouchingObjects().Contains(this.gameObject)
                                            select io).SingleOrDefault();

            if (selectedObj && (selectedObj.transform.parent.name.Equals("MenuRight") || selectedObj.transform.parent.name.Equals("MenuLeft")))
            {
                CreateSelectedObject(selectedObj);
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
        ObjectEvents io = obj.GetComponent<ObjectEvents>();
        io.isUsable = true;
        io.touchHighlightColor = Color.red;
        io.pointerActivatesUseAction = true;
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
        rigidObj.isKinematic = true;

        GameManager.instance.AddInteractableObject(io);
        obj.AddComponent<VRTK_FixedJointGrabAttach>();
    }

    void CreateSelectedObject(ObjectEvents selectedObj)
    {
        GameObject selected = selectedObj.gameObject;
        GameObject newGameObj = Instantiate(selected, selected.transform.position, selected.transform.rotation);
        Guid gameObjName = Guid.NewGuid();
        newGameObj.name = gameObjName.ToString() + "_" + newGameObj.name;
        SetupInteractableObject(newGameObj);
    }
}
