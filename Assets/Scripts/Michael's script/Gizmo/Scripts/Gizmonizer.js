var gizmoAxis: GameObject;
var gizmoSize: float = 1.0;

private var gizmoObj: GameObject;
private var gizmo: Gizmo;
private var gizmoTypea: GizmoTypea = GizmoTypea.Position;

function Update () {
    if (Input.GetKeyDown(KeyCode.Escape)) {
        removeGizmo();
    }
    
    if (gizmo) {
        if (Input.GetKeyDown(KeyCode.Alpha1)) {
            gizmoTypea = GizmoTypea.Position;
            gizmo.setType(gizmoTypea);
        }
        if (Input.GetKeyDown(KeyCode.Alpha2)) {
            gizmoTypea = GizmoTypea.Rotation;
            gizmo.setType(gizmoTypea);
        }
        if (Input.GetKeyDown(KeyCode.Alpha3)) {
            gizmoTypea = GizmoTypea.Scalea;
            gizmo.setType(gizmoTypea);
        }        
        if (gizmo.needUpdate) {
            resetGizmo();
        }
    }
}


function OnMouseDown() {
    if (!gizmoObj) {
        resetGizmo();
    }
}

function removeGizmo() {
    if (gizmoObj) {
        gameObject.layer = 0;
        for (var child : Transform in transform) {
            child.gameObject.layer = 0;
        }        
        Destroy(gizmoObj);    
        Destroy(gizmo);    
    }
}

function resetGizmo() {
    removeGizmo();
    gameObject.layer = 2;
    for (var child : Transform in transform) {
        child.gameObject.layer = 2;
    }        
    gizmoObj = Instantiate(gizmoAxis, transform.position, transform.rotation);
    gizmoObj.transform.localScale *= gizmoSize;
    gizmo = gizmoObj.GetComponent("Gizmo");
    gizmo.setParent(transform);
    gizmo.setType(gizmoTypea);
}

