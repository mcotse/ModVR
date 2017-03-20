using System.Collections;
using System.Collections.Generic;
using System;
using System.Linq;
using UnityEngine;
using VRTK;
using ModVR;

public class GameManager : MonoBehaviour {

    private static GameManager _instance;
    public static int importCounter = 0;
    public string lastSaved = "";
    public List<ModVR_InteractableObject> interactableObjectList;
    public List<ModVR_InteractableObject> selectedObjectList;

    public List<List<string>> collisionSet;
    public bool laserColliding = false;
    public GameObject lastLaserSelectedObj = null;

    public GameObject radialMenu;
    public GameObject objectOptions;
    public static void IncrementImportCounter() {
        importCounter ++;
    }
    public void UpdateLastSaved(string newSavedFileName){
        lastSaved = newSavedFileName;
        Debug.Log( lastSaved);
    }
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

        selectedObjectList = new List<ModVR_InteractableObject>();
        interactableObjectList = new List<ModVR_InteractableObject>();
        

        collisionSet = new List<List<string>>();
        GameObject cube = FindObjectsOfType<ModVR_InteractableObject>().Where(o => o.name == "Cube").First().gameObject;
        cube.GetComponent<ModVR_SelectHighlighter>().Initialise(Color.blue);
    }
	// Update is called once per frame
	void Update () {

	}

    public void AddInteractableObject(ModVR_InteractableObject io)
    {
        interactableObjectList.Add(io);
        //io.InteractableObjectTouched += OnInteractableObjectTouched;
        io.InteractableObjectCollisionEnter += OnInteractableObjectCollision;
        io.InteractableObjectCollisionExit += OnInteractableObjectCollisionExit;

    }

    public bool handleSelectedObject(GameObject obj){
        ModVR_InteractableObject temp = (from o in selectedObjectList
                            where o.name == obj.name 
                            select o).FirstOrDefault();

        if(temp != null){
            selectedObjectList = selectedObjectList.Where(o => o.name != temp.name).ToList();
        }
        else{
            selectedObjectList.Add(obj.GetComponentInChildren<ModVR_InteractableObject>());
        }

        bool selected = (temp == null);

        return selected;
    }

    public void RemoveInteractableObject(ModVR_InteractableObject io)
    {
        interactableObjectList = interactableObjectList.Where(obj => obj.name != io.name).ToList();

        io.InteractableObjectCollisionEnter -= OnInteractableObjectCollision;
        io.InteractableObjectCollisionExit -= OnInteractableObjectCollisionExit;
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

    public void RemoveCollisionByName(string name)
    {
        List<List<string>> collisionsToRemove = (from collision in collisionSet
                                                 where collision.Contains(name)
                                                 select collision).ToList();


        collisionSet = collisionSet.Except(collisionsToRemove).ToList();
    }

    //void OnInteractableObjectTouched(object sender, InteractableObjectEventArgs e)
    //{
    //    VRTK_InteractableObject senderObj = sender as VRTK_InteractableObject;
    //    Debug.Log(senderObj.name);
    //    Debug.Log(e.interactingObject.name);

    //}

    void OnInteractableObjectCollision(object sender, ObjectEventCollisionArgs e)
    {
        GameObject senderObj = sender as GameObject;
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
        ModVR_InteractableObject senderObj = sender as ModVR_InteractableObject;
        GameObject c = e.collider.gameObject;

        if ((c.name == "Controller (right)" || c.name == "Controller (left)") && senderObj != null)
        {
            GameObject go = c.GetComponentInChildren<VRTK_InteractGrab>().GetGrabbedObject();
            if(go != null)
            {
                string senderName = senderObj.name;
                string collider = go.name;

                List<string> collision = new List<string>();
                collision.Add(senderName);
                collision.Add(collider);

                RemoveCollision(collision);
            }
        }
    }


}
