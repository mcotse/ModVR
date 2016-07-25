var axisX: GizmoHandle;
var axisY: GizmoHandle;
var axisZ: GizmoHandle;

var gizmoType: GizmoTypea;

var needUpdate: boolean = false;

function Awake() {
    axisX.axis = GizmoAxis.X;
    axisY.axis = GizmoAxis.Y;
    axisZ.axis = GizmoAxis.Z;
    
    setType(gizmoType);
}


function Update () 
{    
    needUpdate = (axisX.needUpdate || axisY.needUpdate || axisZ.needUpdate);
}

function setType(typea: GizmoTypea) 
{
    axisX.setType(typea);
    axisY.setType(typea);
    axisZ.setType(typea);
}

function setParent(other: Transform) 
{
    transform.parent = other;
    axisX.setParent(other);
    axisY.setParent(other);
    axisZ.setParent(other);
}
