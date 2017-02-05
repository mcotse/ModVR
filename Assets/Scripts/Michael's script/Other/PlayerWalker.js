var speed = 6.0;
var jumpSpeed = 8.0;
var gravity = 20.0;
var rotationSensitivity: float = 1.0;
var lookAngle = 30.0;

private var moveDirection = Vector3.zero;
private var grounded : boolean = false;

var smooth = 2.0;
var tiltAngle = 30.0;

function FixedUpdate() {
    if (grounded) {
        transform.Rotate(Vector3.up * Input.GetAxis("Horizontal") * rotationSensitivity);
        moveDirection = new Vector3(Input.GetAxis("Strafe"), 0, Input.GetAxis("Vertical"));         
        moveDirection = transform.TransformDirection(moveDirection);
        moveDirection *= speed;

        if (Input.GetButton ("Jump")) {
            moveDirection.y = jumpSpeed;
        }
    }

    moveDirection.y -= gravity * Time.deltaTime;

    var controller: CharacterController = GetComponent(CharacterController);
    var flags = controller.Move(moveDirection * Time.deltaTime);
    grounded = (flags & CollisionFlags.CollidedBelow) != 0;
}


@script RequireComponent(CharacterController)