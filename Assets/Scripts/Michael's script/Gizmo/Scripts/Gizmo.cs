using UnityEngine;
using System.Collections;

public class Gizmo : MonoBehaviour 
{
	public GizmoHandle axisX;
	public GizmoHandle axisY;
	public GizmoHandle axisZ;

	public GizmoHandle.Gizmo_Type gizmoType;

	public bool needUpdate = false;

	void OnAwake() 
	{
		axisX.axis = GizmoHandle.GizmoAxis.X;
		axisY.axis = GizmoHandle.GizmoAxis.Y;
		axisZ.axis = GizmoHandle.GizmoAxis.Z;
		
		setType(gizmoType);
	}

	void OnUpdate () 
	{    
		needUpdate = (axisX.needUpdate || axisY.needUpdate || axisZ.needUpdate);
	}

	public void setType(GizmoHandle.Gizmo_Type typea) 
	{
		axisX.setType(typea);
		axisY.setType(typea);
		axisZ.setType(typea);
	}

	public void setParent(Transform other) 
	{
		transform.parent = other;
		axisX.setParent(other);
		axisY.setParent(other);
		axisZ.setParent(other);
	}
}
