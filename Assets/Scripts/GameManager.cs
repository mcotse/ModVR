using System.Collections;
using System.Collections.Generic;
using System;
using System.Linq;
using UnityEngine;
using VRTK;
using ModVR;

public class GameManager : MonoBehaviour {

    private static GameManager _instance;

    public List<VRTK_InteractableObject> interactableObjectList;
    public List<VRTK_InteractableObject> selectedObjectList;

    public List<List<string>> collisionSet;
    public bool laserColliding = false;
    public GameObject lastLaserSelectedObj = null;

    public static GameManager instance {
        get
        {
            return _instance;
        }
    }
    // Use this for initialization
    void Awake () {
        init();
	}

    void init() {

        if (instance != _instance)
        {
            Destroy(gameObject);
        }
        else
        {
            _instance = this;
        }

        selectedObjectList = new List<VRTK_InteractableObject>();
        interactableObjectList = new List<VRTK_InteractableObject>();
        

        collisionSet = new List<List<string>>();
    }
	// Update is called once per frame
	void Update () {

	}

    public void AddInteractableObject(ObjectEvents io)
    {
        interactableObjectList.Add(io);
        //io.InteractableObjectTouched += OnInteractableObjectTouched;
        io.InteractableObjectCollisionEnter += OnInteractableObjectCollision;
        io.InteractableObjectCollisionExit += OnInteractableObjectCollisionExit;

    }
    public void handleSelectedObject(GameObject obj){
        VRTK_InteractableObject temp = (from o in selectedObjectList
                            where o.name == obj.name 
                            select o).FirstOrDefault();

        if(temp != null){
            selectedObjectList = selectedObjectList.Where(o => o.name != temp.name).ToList();
        }
        else{
            selectedObjectList.Add(obj.GetComponentInChildren<VRTK_InteractableObject>());
        }
    }

    public void RemoveInteractableObject(VRTK_InteractableObject io)
    {
        interactableObjectList.Remove(io);
    }

    public void AddCollision(List<string> collision)
    {
        
        collision.Sort();

        foreach(List<string> col in collisionSet)
        {
            if (col.SequenceEqual(collision))
            {
                return;
            }
        }

        collisionSet.Add(collision);
    }

    public void RemoveCollision(List<string> collision)
    {
        collision.Sort();

        List<string> temp = null;
        foreach (List<string> tup in collisionSet)
        {
            if (collision.Equals(tup))
            {
                temp = tup;
            }
        }

        if (temp != null)
        {
            collisionSet.Remove(temp);
        }
    }

    //void OnInteractableObjectTouched(object sender, InteractableObjectEventArgs e)
    //{
    //    VRTK_InteractableObject senderObj = sender as VRTK_InteractableObject;
    //    Debug.Log(senderObj.name);
    //    Debug.Log(e.interactingObject.name);

    //}

    void OnInteractableObjectCollision(object sender, ObjectEventCollisionArgs e)
    {
        VRTK_InteractableObject senderObj = sender as VRTK_InteractableObject;
        GameObject c = e.collider.gameObject;
        if (c.name == "Controller (right)" || c.name == "Controller (left)")
        {
            GameObject go = c.GetComponentInChildren<VRTK_InteractGrab>().GetGrabbedObject();
            if (go != null)
            {
                string senderName = senderObj.name;
                string collider = go.name;

                List<string> collision = new List<string>();
                collision.Add(senderName);
                collision.Add(collider);
                collision.Sort();

                AddCollision(collision);
            }
        }
    }

    void OnInteractableObjectCollisionExit(object sender, ObjectEventCollisionArgs e)
    {
        VRTK_InteractableObject senderObj = sender as VRTK_InteractableObject;
        GameObject c = e.collider.gameObject;

        if (c.name == "Controller (right)" || c.name == "Controller (left)")
        {
            Debug.Log("Controller Object type" + c.GetType() + ". Controller name: " + c.name);
            GameObject go = c.GetComponentInChildren<VRTK_InteractGrab>().GetGrabbedObject();
            if(go != null)
            {
                string senderName = senderObj.name;
                string collider = go.name;

                List<string> collision = new List<string>();
                collision.Add(senderName);
                collision.Add(collider);

                RemoveCollision(collision);

                Debug.Log("Exit Collision with: " + senderName);
                Debug.Log("Exit Collider: " + collider);
            }
        }
    }


}
