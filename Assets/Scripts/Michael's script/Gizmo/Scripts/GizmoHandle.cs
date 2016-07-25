using UnityEngine;
using System.Collections;

public class GizmoHandle : MonoBehaviour 
{
	public GameObject positionEnd;
	public GameObject rotationEnd;
	public GameObject scaleEnd;
	
	public float moveSensitivity = 6;
	public float rotationSensitivity = 64;
	public bool needUpdate = false;

	public enum GizmoControl {Horizontal, Vertical, Both}
	public enum Gizmo_Type {Gizmo_Position, Gizmo_Rotation, Gizmo_Scale} 
	public enum GizmoAxis {X, Y, Z}

	public Gizmo_Type gizmo_type = Gizmo_Type.Gizmo_Position;
	public GizmoControl control = GizmoControl.Both;
	public GizmoAxis axis = GizmoAxis.X;

	private bool mouseDown = false;
	private Transform otherTrans;

	void OnAwake() 
	{
		otherTrans = transform.parent;
	}

	public void setParent(Transform other) 
	{
		otherTrans = other;    
	}

	public void setType(Gizmo_Type gizmotype) 
	{
		this.gizmo_type = gizmotype;
		if ( gizmotype == Gizmo_Type.Gizmo_Position )
			positionEnd.active = true;
		else
			positionEnd.active = false;
		
		if ( gizmotype == Gizmo_Type.Gizmo_Rotation )
			rotationEnd.active = true ;
		else
			rotationEnd.active = false ;
		
		if ( gizmotype == Gizmo_Type.Gizmo_Scale )
			scaleEnd.active = true;
		else
			scaleEnd.active = false;
	}

	void OnMouseDown() 
	{
		mouseDown = true;
	}

	void OnMouseUp() 
	{
		mouseDown = false;
		needUpdate = true;
	}


	void OnMouseDrag() 
	{
		float delta = 0.0f;
		
		if (mouseDown) 
		{
			switch (control) 
			{
				case GizmoControl.Horizontal:
					delta = Input.GetAxis("Mouse X") * Time.deltaTime; 
					break;
				case GizmoControl.Vertical:
					delta = Input.GetAxis("Mouse Y") * Time.deltaTime; 
				Debug.Log("control Y Axis");
					break;
				case GizmoControl.Both:
					delta = (Input.GetAxis("Mouse X") + Input.GetAxis("Mouse Y")) * Time.deltaTime; 
					break;
			}

        
			switch (gizmo_type) 
			{
				case Gizmo_Type.Gizmo_Position:
					delta *= moveSensitivity;
				
					switch (axis) 
					{
						case GizmoAxis.X:
							otherTrans.Translate(Vector3.right * delta);
							break;
						case GizmoAxis.Y:
							otherTrans.Translate(Vector3.up * delta);
							break;
						case GizmoAxis.Z:
							otherTrans.Translate(Vector3.forward * delta);
							break;
					}
				
					break;

				case Gizmo_Type.Gizmo_Scale:
					delta *= moveSensitivity;
					switch (axis) 
					{
						case GizmoAxis.X:
							otherTrans.localScale  += new Vector3(delta,0f,0f);
							break;
						case GizmoAxis.Y:
							otherTrans.localScale += new Vector3(0f,delta,0f);
							break;
						case GizmoAxis.Z:
							otherTrans.localScale += new Vector3(0f,0f,delta);
							break;
					}
					break;
					
				case Gizmo_Type.Gizmo_Rotation:
					delta *= rotationSensitivity;
					switch (axis) 
					{
						case GizmoAxis.X:
							otherTrans.Rotate(Vector3.right * delta);
							break;
						case GizmoAxis.Y:
							otherTrans.Rotate(Vector3.up * delta);
							break;
						case GizmoAxis.Z:
							otherTrans.Rotate(Vector3.forward * delta);
							break;
					}
					break;
			}
		}
	}

}
