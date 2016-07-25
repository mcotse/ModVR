using UnityEngine;
using System.Collections;

public class MenuController : MonoBehaviour {

	public GameObject cube;
	private GameObject menuCube;
	private bool menuCubeExists;

	// Use this for initialization
	void Start () {
		menuCubeExists = false;
		//cube = this.transform.Find ("MenuCube").gameObject;
		//foreach (Transform child in transform)
		//	Debug.Log (child.ToString ());
	}
	
	// Update is called once per frame
	void Update () {
		Transform menuCubeTransform = this.transform.Find ("MenuCube");
		GameObject newCube;
		if (menuCubeTransform == null && !menuCubeExists) {
			newCube = Instantiate (cube);
			newCube.transform.SetParent (this.transform);
			menuCubeExists = true;
		}
	}
}
