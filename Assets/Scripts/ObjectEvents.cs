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

    private void OnCollisionStay(Collision collision)
    {
        //List<string> names = new List<string>();
        //names.Add(this.name);
        //names.Add(collision.gameObject.name);
        //SendMessage("OnCollisionStayEvent", names);
    }

    private void OnTriggerStay(Collider other)
    {
        //if(other.transform.tag == )
    }
}
