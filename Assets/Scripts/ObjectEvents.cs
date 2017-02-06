using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectEvents : MonoBehaviour {

    public bool isSelected = true;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    void OnCollisionStay(Collision collision)
    {
        Debug.Log("Collision detected for: " + this.name + " with " + collision.gameObject.name);
        List<string> names = new List<string>();
        names.Add(this.name);
        names.Add(collision.gameObject.name); 
        SendMessage("OnCollisionStayEvent", names);
    }
}
