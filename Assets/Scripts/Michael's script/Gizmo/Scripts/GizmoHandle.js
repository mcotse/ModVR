var positionEnd: GameObject;
var rotationEnd: GameObject;
var scaleEnd: GameObject;
var moveSensitivity: float = 6;
var rotationSensitivity: float = 64;
var needUpdate: boolean = false;

enum GizmoControl {Horizontal, Vertical, Both}
enum GizmoTypea {Position, Rotation, Scalea} 
enum GizmoAxis {X, Y, Z}

var typea: GizmoTypea = GizmoTypea.Position;
var control: GizmoControl = GizmoControl.Both;
var axis: GizmoAxis = GizmoAxis.X;

private var mouseDown: boolean = false;
private var otherTrans: Transform;

function Awake() {
    otherTrans = transform.parent;
}

function Update () {

}

function setParent(other: Transform) {
    otherTrans = other;    
}

function setType(typea: GizmoTypea) {
    this.typea = typea;
    positionEnd.active = typea == GizmoTypea.Position;
    rotationEnd.active = typea == GizmoTypea.Rotation;
    scaleEnd.active = typea == GizmoTypea.Scalea;
}

function OnMouseDown() {
    mouseDown = true;
}

function OnMouseUp() {
    mouseDown = false;
    needUpdate = true;
}


function OnMouseDrag() {
    var delta;
    if (mouseDown) {
        switch (control) {
        case GizmoControl.Horizontal:
            delta = Input.GetAxis("Mouse X") * Time.deltaTime; 
            break;
        case GizmoControl.Vertical:
            delta = Input.GetAxis("Mouse Y") * Time.deltaTime; 
            break;
        case GizmoControl.Both:
            delta = (Input.GetAxis("Mouse X") + Input.GetAxis("Mouse Y")) * Time.deltaTime; 
            break;
        }

        
        switch (typea) {
        case GizmoTypea.Position:
            delta *= moveSensitivity;
        
            switch (axis) {
            case GizmoAxis.X:
                otherTrans.Translate(Vector3.right * delta);
				Debug.Log("X Axis");
                break;
            case GizmoAxis.Y:
                otherTrans.Translate(Vector3.up * delta);
				Debug.Log("Y Axis");
                break;
            case GizmoAxis.Z:
                otherTrans.Translate(Vector3.forward * delta);
				Debug.Log("Z Axis");
                break;
            }
            
            break;

        case GizmoTypea.Scalea:
            delta *= moveSensitivity;
            switch (axis) {
            case GizmoAxis.X:
                otherTrans.localScale.x += delta;
                break;
            case GizmoAxis.Y:
                otherTrans.localScale.y += delta;
                break;
            case GizmoAxis.Z:
                otherTrans.localScale.z += delta;
                break;
            }
            break;
            
        case GizmoTypea.Rotation:
            delta *= rotationSensitivity;
            switch (axis) {
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
        }
    }
}


