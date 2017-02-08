using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using VRTK;
using VRTK.SecondaryControllerGrabActions;
using VRTK.GrabAttachMechanics;
using VRTK.Highlighters;

public class ModVR_WandController : MonoBehaviour {

    public GameObject menu;

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

        actions = GetComponent<VRTK_ControllerActions>();
        events = GetComponent<VRTK_ControllerEvents>();
        
        events.ButtonOnePressed += OnMenuButtonPressed;
        events.TouchpadPressed += OnTouchpadPressed;
        events.TriggerClicked += OnTriggerClicked;
        
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
            ObjectEvents iObj = (from io in GameObject.FindObjectsOfType<ObjectEvents>()
                                            where io.IsTouched() && io.GetTouchingObjects().Contains(this.gameObject)
                                            select io).SingleOrDefault();
            if (iObj && (iObj.transform.parent.name.Equals("MenuRight") || iObj.transform.parent.name.Equals("MenuLeft")))
            {
                GameObject selected = iObj.gameObject;
                GameObject newGameObj = Instantiate(selected, selected.transform.position, selected.transform.rotation);
                SetupInteractableObject(newGameObj);
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
        io.grabAttachMechanicScript = obj.AddComponent<VRTK_ChildOfControllerGrabAttach>();
        io.secondaryGrabActionScript = obj.AddComponent<VRTK_AxisScaleGrabAction>();
        
        

        Rigidbody rigidObj = obj.GetComponent<Rigidbody>();
        rigidObj.constraints = RigidbodyConstraints.None;
        rigidObj.useGravity = false;
        rigidObj.isKinematic = true;

        GameManager.instance.AddInteractableObject(io);
        obj.AddComponent<VRTK_FixedJointGrabAttach>();
    }
}
