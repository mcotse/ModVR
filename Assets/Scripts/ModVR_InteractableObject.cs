﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VRTK;
using ModVR;

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
        
        if (other.gameObject.name.Contains("BasePointer_ObjectInteractor_Collider"))
        {
            GameManager.instance.laserColliding = true;
            GameManager.instance.lastLaserSelectedObj = gameObject;
        }

        if (InteractableObjectCollisionEnter != null)
        {
            ObjectEventCollisionArgs e = new ObjectEventCollisionArgs();
            e.collider = other;
            InteractableObjectCollisionEnter(this, e);
        }

        
    }

    private void OnTriggerExit(Collider other)
    {
		
        if (InteractableObjectCollisionExit != null)
        {
            ObjectEventCollisionArgs e = new ObjectEventCollisionArgs();
            e.collider = other;
            InteractableObjectCollisionExit(this, e);
        }
		GameManager.instance.laserColliding = false;
    }
}