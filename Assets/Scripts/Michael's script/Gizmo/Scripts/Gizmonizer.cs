using UnityEngine;
using System.Collections;

public class Gizmonizer : MonoBehaviour 
{
	public GameObject gizmoAxis;
	public float gizmoSize = 1.0f;

	private GameObject gizmoObj;
	private Gizmo gizmo;
	private GizmoHandle.Gizmo_Type gizmo_type = GizmoHandle.Gizmo_Type.Gizmo_Position;

	void Update () 
	{
		if (Input.GetKeyDown(KeyCode.Escape)) 
			removeGizmo();
		
		if (gizmo) 
		{
			if (Input.GetKeyDown(KeyCode.Alpha1)) 
			{
				gizmo_type = GizmoHandle.Gizmo_Type.Gizmo_Position;
				gizmo.setType(gizmo_type);
			}
			if (Input.GetKeyDown(KeyCode.Alpha2)) {
				gizmo_type = GizmoHandle.Gizmo_Type.Gizmo_Rotation;
				gizmo.setType(gizmo_type);
			}
			if (Input.GetKeyDown(KeyCode.Alpha3)) {
				gizmo_type = GizmoHandle.Gizmo_Type.Gizmo_Scale;
				gizmo.setType(gizmo_type);
			}        
			if (gizmo.needUpdate) {
				resetGizmo();
			}
		}
	}


	void OnMouseDown() 
	{
		if (!gizmoObj) {
			resetGizmo();
		}
	}

	void removeGizmo() 
	{
		if (gizmoObj) 
		{
			gameObject.layer = 0;
			foreach (Transform child in transform) 
			{
				child.gameObject.layer = 0;
			}        
			Destroy(gizmoObj);    
			Destroy(gizmo);    
		}
	}

	void resetGizmo() 
	{
		removeGizmo();
		gameObject.layer = 2;
		foreach (Transform child in transform) 
		{
			child.gameObject.layer = 2;
		}        
		gizmoObj = Instantiate(gizmoAxis, transform.position, transform.rotation) as GameObject;
		gizmoObj.transform.localScale *= gizmoSize;
		gizmo = gizmoObj.GetComponent<Gizmo>();
		gizmo.setParent(transform);
		gizmo.setType(gizmo_type);
	}

}
