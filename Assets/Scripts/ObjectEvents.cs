﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VRTK;
using ModVR;

public class ObjectEvents : VRTK_InteractableObject {
    
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

    private void OnCollisionEnter(Collision collision)
    {
        if(controllerActions && IsGrabbed())
        {
            ObjectEventCollisionArgs e = new ObjectEventCollisionArgs();
            e.collision = collision;
            InteractableObjectCollisionEnter(this, e);
        }
    }

    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        //if (controllerActions && IsGrabbed())
        //{
        //    ObjectEventCollisionArgs e = new ObjectEventCollisionArgs();
        //    e.colliderHit = hit;
        //    InteractableObjectCollisionEnter(this, e);
        //}
    }

    private void OnTriggerEnter(Collider other)
    {
        ObjectEventCollisionArgs e = new ObjectEventCollisionArgs();
        e.collider = other;
        InteractableObjectCollisionEnter(this, e);
    }

    private void OnTriggerExit(Collider other)
    {
        ObjectEventCollisionArgs e = new ObjectEventCollisionArgs();
        e.collider = other;
        InteractableObjectCollisionExit(this, e);
    }

    private void OnTriggerStay(Collider other)
    {
        //if(other.transform.tag == )
    }
}
