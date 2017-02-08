using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VRTK;
using ModVR;

public class GameManager : MonoBehaviour {

    public static GameManager instance = null;



    public List<VRTK_InteractableObject> interactableObjectList;
    public List<VRTK_InteractableObject> selectedObjectList;
    public List<VRTK_InteractableObject> collidingObjectList;
	// Use this for initialization
	void Awake () {
        init();
	}

    void init() {
        if(instance == null)
        {
            instance = this;
            selectedObjectList = new List<VRTK_InteractableObject>();
            collidingObjectList = new List<VRTK_InteractableObject>();
            interactableObjectList = new List<VRTK_InteractableObject>();
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }
    }
	// Update is called once per frame
	void Update () {
		
	}

    public void AddInteractableObject(ObjectEvents io)
    {
        interactableObjectList.Add(io);
        io.InteractableObjectTouched += OnInteractableObjectTouched;
        io.InteractableObjectCollisionEnter += OnInteractableObjectCollision;
        io.InteractableObjectCollisionExit += OnInteractableObjectCollisionExit;
        
    }

    public void RemoveInteractableObject(VRTK_InteractableObject io)
    {
        interactableObjectList.Remove(io);
    }

    
    void OnInteractableObjectTouched(object sender, InteractableObjectEventArgs e)
    {
        VRTK_InteractableObject senderObj = sender as VRTK_InteractableObject;
        Debug.Log(senderObj.name);
        Debug.Log(e.interactingObject.name);
        
    }

    void OnInteractableObjectCollision(object sender, ObjectEventCollisionArgs e)
    {
        VRTK_InteractableObject senderObj = sender as VRTK_InteractableObject;
        Debug.Log("Sender object" + senderObj.name);
        Debug.Log("Collider object" + e.collider.gameObject.transform.parent.name);

    }

    void OnInteractableObjectCollisionExit(object sender, ObjectEventCollisionArgs e)
    {
        VRTK_InteractableObject senderObj = sender as VRTK_InteractableObject;
        Debug.Log("Sender object" + senderObj.name);
        Debug.Log("Collider object" + e.collider.gameObject.transform.parent.name);

    }
}
