using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VRTK;
using ModVR;
using System.Linq;
using VRTK.Highlighters;

public class ModVR_InteractableObject : VRTK_InteractableObject {
    
    public delegate void InteractableObjectEventHandler(object sender, ObjectEventCollisionArgs e);

    public event InteractableObjectEventHandler InteractableObjectCollisionEnter;
    public event InteractableObjectEventHandler InteractableObjectCollisionExit;
    
    private VRTK_ControllerActions controllerActions;

    public override void Grabbed(GameObject grabbingObject)
    {
        base.Grabbed(grabbingObject);
        controllerActions = grabbingObject.GetComponent<VRTK_ControllerActions>();
    }

    protected override void Awake()
    {
        base.Awake();
        interactableRigidbody.collisionDetectionMode = CollisionDetectionMode.Continuous;
    }

    public override void OnInteractableObjectTouched(InteractableObjectEventArgs e)
    {
        base.OnInteractableObjectTouched(e);

    }

    private void OnCollisionEnter(Collision collision)
    {
    }
    //private void OnCollisionEnter(Collision collision)
    //{
    //    //    if (controllerActions && IsGrabbed())
    //    //    {
    //    //        ObjectEventCollisionArgs e = new ObjectEventCollisionArgs();
    //    //        e.collision = collision;
    //    //        InteractableObjectCollisionEnter(this, e);
    //    //    }
    //    Debug.Log(collision.gameObject.name);
    //    if (collision.gameObject.name.Contains("BasePointer_ObjectInteractor_Collider")){
    //        GameManager.instance.laserColliding = true;
    //        GameManager.instance.lastLaserSelectedObj = this.gameObject;
    //    }
    //}

    //private void OnCollisionExit(Collision collision)
    //{


    //}

    private void OnTriggerEnter(Collider other)
    {
        bool isPointer = other.gameObject.name.Contains("BasePointer_ObjectInteractor_Collider");
        if (isPointer)
        {
            GameManager.instance.laserColliding = true;
            GameManager.instance.lastLaserSelectedObj = gameObject;

            if (gameObject.name.Contains("groupObj"))
            {
                List<GameObject> children = GetChildren();
                foreach(GameObject child in children)
                {
                    VRTK_OutlineObjectCopyHighlighter highlighter = child.GetComponent<VRTK_OutlineObjectCopyHighlighter>();
                    highlighter.Highlight(Color.red);
                }
            }
        }



        if (InteractableObjectCollisionEnter != null && !isPointer)
        {
            ObjectEventCollisionArgs e = new ObjectEventCollisionArgs();
            e.collider = other;
            InteractableObjectCollisionEnter(gameObject, e);
        }


    }

    private void OnTriggerExit(Collider other)
    {
		
        if (InteractableObjectCollisionExit != null)
        {
            ObjectEventCollisionArgs e = new ObjectEventCollisionArgs();
            e.collider = other;
            InteractableObjectCollisionExit(gameObject, e);
        }

        bool isPointer = other.gameObject.name.Contains("BasePointer_ObjectInteractor_Collider");
        if (isPointer && gameObject.name.Contains("groupObj"))
        {
            List<GameObject> children = GetChildren();
            foreach (GameObject child in children)
            {
                VRTK_OutlineObjectCopyHighlighter highlighter = child.GetComponent<VRTK_OutlineObjectCopyHighlighter>();
                highlighter.Unhighlight(Color.clear);
            }
        }
        GameManager.instance.laserColliding = false;
    }


    private List<GameObject> GetChildren()
    {
        List<GameObject> children = (from Transform t in gameObject.transform
                                     where t.name.Contains("Highlight") == false
                                     select t.gameObject).ToList();

        return children;
    }
}
