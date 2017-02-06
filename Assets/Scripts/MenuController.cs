﻿using UnityEngine;
using System.Collections;

public class MenuController : MonoBehaviour {

	public GameObject cube;
	public GameObject sphere;
	//private GameObject menuCube;
	//private bool menuCubeExists;

	// Use this for initialization
	void Start () {
		//menuCubeExists = false;
		//cube = this.transform.Find ("MenuCube").gameObject;
		//foreach (Transform child in transform)
		//	Debug.Log (child.ToString ());
	}
	
	// Update is called once per frame
	void Update () {
		Transform menuCubeTransform = this.transform.Find ("MenuCube");
		Transform menuSphereTransform = this.transform.Find ("MenuSphere");
		GameObject newCube;
		GameObject newSphere;
		if (menuCubeTransform == null) {
			newCube = Instantiate (cube);
			newCube.transform.SetParent (this.transform);
			newCube.transform.position = this.transform.position + new Vector3(-0.075f, -0.1060f, -0.0353f);
			newCube.name = "MenuCube";
            newCube.AddComponent<ObjectEvents>();
			//menuCubeExists = true;
		}
		if (menuSphereTransform == null) {
			newSphere = Instantiate (sphere);
			newSphere.transform.SetParent (this.transform);
			newSphere.transform.position = this.transform.position + new Vector3(0.075f, -0.1060f, -0.0353f);
			newSphere.name = "MenuSphere";
            newSphere.AddComponent<ObjectEvents>();
		}
	}
}
