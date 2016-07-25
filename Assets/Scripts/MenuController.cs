using UnityEngine;
using System.Collections;

public class MenuController : MonoBehaviour {

	public GameObject cube;

	// Use this for initialization
	void Start () {
		//cube = transform.Find ("Cube").gameObject;
	}
	
	// Update is called once per frame
	void Update () {
		if (transform.Find ("MenuCube").gameObject == null)
			Debug.Log ("Cube NOT found");
		else
			Debug.Log ("Cube found");
	}
}
