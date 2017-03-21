// Axis Scale Grab Action|SecondaryControllerGrabActions|60030
namespace VRTK.SecondaryControllerGrabActions
{
    using UnityEngine;

    /// <summary>
    /// The Axis Scale Grab Action provides a mechanism to scale objects when they are grabbed with a secondary controller.
    /// </summary>
    /// <example>
    /// `VRTK/Examples/043_Controller_SecondaryControllerActions` demonstrates the ability to grab an object with one controller and scale it by grabbing and pulling with the second controller.
    /// </example>
    public class VRTK_AxisScaleGrabAction : VRTK_BaseGrabAction
    {
        [Tooltip("The distance the secondary controller must move away from the original grab position before the secondary controller auto ungrabs the object.")]
        public float ungrabDistance = 1f;
        [Tooltip("If checked the current X Axis of the object won't be scaled")]
        public bool lockXAxis = false;
        [Tooltip("If checked the current Y Axis of the object won't be scaled")]
        public bool lockYAxis = false;
        [Tooltip("If checked the current Z Axis of the object won't be scaled")]
        public bool lockZAxis = false;
        [Tooltip("If checked all the axes will be scaled together (unless locked)")]
        public bool uniformScaling = false;

        private Vector3 initialScale;
        private float initalLength;
        private float initialScaleFactor;
        private Vector3 lastPos; 
       

        /// <summary>
        /// The Initalise method is used to set up the state of the secondary action when the object is initially grabbed by a secondary controller.
        /// </summary>
        /// <param name="currentGrabbdObject">The Interactable Object script for the object currently being grabbed by the primary controller.</param>
        /// <param name="currentPrimaryGrabbingObject">The Interact Grab script for the object that is associated with the primary controller.</param>
        /// <param name="currentSecondaryGrabbingObject">The Interact Grab script for the object that is associated with the secondary controller.</param>
        /// <param name="primaryGrabPoint">The point on the object where the primary controller initially grabbed the object.</param>
        /// <param name="secondaryGrabPoint">The point on the object where the secondary controller initially grabbed the object.</param>
        public override void Initialise(VRTK_InteractableObject currentGrabbdObject, VRTK_InteractGrab currentPrimaryGrabbingObject, VRTK_InteractGrab currentSecondaryGrabbingObject, Transform primaryGrabPoint, Transform secondaryGrabPoint)
        {
            base.Initialise(currentGrabbdObject, currentPrimaryGrabbingObject, currentSecondaryGrabbingObject, primaryGrabPoint, secondaryGrabPoint);
            initialScale = currentGrabbdObject.transform.localScale;
            initalLength = (grabbedObject.transform.position - secondaryGrabbingObject.transform.position).magnitude;
            initialScaleFactor = currentGrabbdObject.transform.localScale.x / initalLength;
            lastPos = (GameObject.Find("Controller (right)").transform.position - GameObject.Find("Controller (left)").transform.position);
        }

        /// <summary>
        /// The ProcessUpdate method runs in every Update on the Interactable Object whilst it is being grabbed by a secondary controller.
        /// </summary>
        public override void ProcessUpdate()
        {
            CheckForceStopDistance(ungrabDistance);
        }

        /// <summary>
        /// The ProcessFixedUpdate method runs in every FixedUpdate on the Interactable Object whilst it is being grabbed by a secondary controller and performs the scaling action.
        /// </summary>
        public override void ProcessFixedUpdate()
        {
            if (initialised)
            {
                if (uniformScaling)
                {
                    UniformScale();
                }
                else
                {
                    NonUniformScale();
                }
            }
        }

        private void ApplyScale(Vector3 newScale)
        {
            var existingScale = grabbedObject.transform.localScale;

            float finalScaleX = (lockXAxis ? existingScale.x : newScale.x);
            float finalScaleY = (lockYAxis ? existingScale.y : newScale.y);
            float finalScaleZ = (lockZAxis ? existingScale.z : newScale.z);

            if (finalScaleX > 0 && finalScaleY > 0 && finalScaleZ > 0)
            {
                grabbedObject.transform.localScale = new Vector3(finalScaleX, finalScaleY, finalScaleZ); ;
            }
            if (grabbedObject.GetComponent<MeshCollider>() != null)
            {
                Destroy(grabbedObject.GetComponent<MeshCollider>());
            } 
            if (grabbedObject.GetComponent<BoxCollider>() != null)
            {
                Destroy(grabbedObject.GetComponent<BoxCollider>());
            }
            grabbedObject.gameObject.AddComponent<MeshCollider>();
        }

        private void NonUniformScale()
        {
            /*Vector3 initialRotatedPosition = grabbedObject.transform.rotation * grabbedObject.transform.position;
            Vector3 initialSecondGrabRotatedPosition = grabbedObject.transform.rotation * secondaryInitialGrabPoint.position;
            Vector3 currentSecondGrabRotatedPosition = grabbedObject.transform.rotation * secondaryGrabbingObject.transform.position;

            float newScaleX = CalculateAxisScale(initialRotatedPosition.x, initialSecondGrabRotatedPosition.x, currentSecondGrabRotatedPosition.x);
            float newScaleY = CalculateAxisScale(initialRotatedPosition.y, initialSecondGrabRotatedPosition.y, currentSecondGrabRotatedPosition.y);
            float newScaleZ = CalculateAxisScale(initialRotatedPosition.z, initialSecondGrabRotatedPosition.z, currentSecondGrabRotatedPosition.z);

            var newScale = new Vector3(newScaleX, newScaleY, newScaleZ) + initialScale;
            ApplyScale(newScale);

              float max = Mathf.Max(Mathf.Max(Mathf.Abs(velocity.x), Mathf.Abs(velocity.y)), Mathf.Abs(velocity.z));

            if (max == Mathf.Abs(velocity.x))
            {
                newScale = selected.transform.localScale + new Vector3(velocity.x * scalingFactor, 0, 0);
            }
            else if (max == Mathf.Abs(velocity.y))
            {
                newScale = selected.transform.localScale + new Vector3(0, velocity.y * scalingFactor, 0);
            }
            else if (max == Mathf.Abs(velocity.z))
            {
                newScale = selected.transform.localScale + new Vector3(0, 0, velocity.z * scalingFactor);
            }*/
            /*Vector3 initialRotatedPosition = grabbedObject.transform.rotation * grabbedObject.transform.position;
            Vector3 initialSecondGrabRotatedPosition = grabbedObject.transform.rotation * secondaryInitialGrabPoint.position;
            Vector3 currentSecondGrabRotatedPosition = grabbedObject.transform.rotation * secondaryGrabbingObject.transform.position;
            float newScaleX = CalculateAxisScale(initialRotatedPosition.x, initialSecondGrabRotatedPosition.x, currentSecondGrabRotatedPosition.x);
            float newScaleY = CalculateAxisScale(initialRotatedPosition.y, initialSecondGrabRotatedPosition.y, currentSecondGrabRotatedPosition.y);
            float newScaleZ = CalculateAxisScale(initialRotatedPosition.z, initialSecondGrabRotatedPosition.z, currentSecondGrabRotatedPosition.z);

            float max = Mathf.Max(Mathf.Max(newScaleX, newScaleY), newScaleZ);

            

            if (max == newScaleX)
            {
                newScale += new Vector3(newScaleX, 0, 0);
            }
            else if (max == newScaleY)
            {
                newScale += new Vector3(0, newScaleY, 0);
            }
            else if( max == newScaleZ)
            {
                newScale += new Vector3(0, 0, newScaleZ);
            }
            
            ApplyScale(newScale);!*/
            Vector3 initialRotatedPosition = grabbedObject.transform.rotation * grabbedObject.transform.position;
            Vector3 initialSecondGrabRotatedPosition = grabbedObject.transform.rotation * secondaryInitialGrabPoint.position;
            Vector3 currentSecondGrabRotatedPosition = grabbedObject.transform.rotation * secondaryGrabbingObject.transform.position;
            /*float newScaleX = CalculateAxisScale(initialRotatedPosition.x, initialSecondGrabRotatedPosition.x, currentSecondGrabRotatedPosition.x);
            float newScaleY = CalculateAxisScale(initialRotatedPosition.y, initialSecondGrabRotatedPosition.y, currentSecondGrabRotatedPosition.y);
            float newScaleZ = CalculateAxisScale(initialRotatedPosition.z, initialSecondGrabRotatedPosition.z, currentSecondGrabRotatedPosition.z);*/
            //Debug.Log("newScaleX " + newScaleX + " newScaleY" + newScaleY + " newScaleZ "+ newScaleZ);
            // Debug.Log(GameObject.Find("Controller (left)").transform.position);


            /*if (!flag) {
                initialDif = GameObject.Find("Controller (left)").transform.position - GameObject.Find("Controller (right)").transform.position;
                flag = !flag; 
            }*/
            Vector3 GrabbingDirection = (GameObject.Find("Controller (right)").transform.position - GameObject.Find("Controller (left)").transform.position);
            Debug.Log("GrabbingDirection " + GrabbingDirection + "lastpos" + lastPos);
            GrabbingDirection = GrabbingDirection - lastPos;



            //float abs = Mathf.Sqrt(Mathf.Pow(GrabbingDirection.x, 2) + Mathf.Pow(GrabbingDirection.x, 2) + Mathf.Pow(GrabbingDirection.x, 2));
            float f = Mathf.Deg2Rad * grabbedObject.transform.rotation.eulerAngles.y; 
           // Debug.Log("eulerAngles.y" + Mathf.Cos(5.2F));
            float newScaleX = GrabbingDirection.x+ Mathf.Cos(Mathf.Deg2Rad*grabbedObject.transform.rotation.eulerAngles.y) * GrabbingDirection.x - Mathf.Sin(Mathf.Deg2Rad*grabbedObject.transform.rotation.eulerAngles.y) * GrabbingDirection.z + Mathf.Cos(Mathf.Deg2Rad*grabbedObject.transform.rotation.eulerAngles.z) * GrabbingDirection.x - Mathf.Sin(Mathf.Deg2Rad*grabbedObject.transform.rotation.eulerAngles.z) * GrabbingDirection.y; 
            float newScaleY =  GrabbingDirection.y + Mathf.Sin(Mathf.Deg2Rad*grabbedObject.transform.rotation.eulerAngles.x) * GrabbingDirection.z + Mathf.Cos(Mathf.Deg2Rad*grabbedObject.transform.rotation.eulerAngles.x) * GrabbingDirection.y - Mathf.Sin(Mathf.Deg2Rad*grabbedObject.transform.rotation.eulerAngles.z) * GrabbingDirection.x + Mathf.Cos(Mathf.Deg2Rad*grabbedObject.transform.rotation.eulerAngles.z) * GrabbingDirection.y;
            //Debug.Log("newScaleX " + newScaleX + " newScaleY" + newScaleY + " newScaleZ " + newScaleZ);
            float newScaleZ = GrabbingDirection.z + Mathf.Sin(Mathf.Deg2Rad*grabbedObject.transform.rotation.eulerAngles.y) * GrabbingDirection.x + Mathf.Cos(Mathf.Deg2Rad*grabbedObject.transform.rotation.eulerAngles.y) * GrabbingDirection.z + Mathf.Cos(Mathf.Deg2Rad*grabbedObject.transform.rotation.eulerAngles.x) * GrabbingDirection.z - Mathf.Sin(Mathf.Deg2Rad*grabbedObject.transform.rotation.eulerAngles.x) * GrabbingDirection.y;
           // Debug.Log("newScaleX " + newScaleX + " newScaleY" + newScaleY + " newScaleZ " + newScaleZ);
            //Debug.Log("newScaleX " + newScaleX + " secondaryInitialGrabPoint.position " + secondaryInitialGrabPoint.position + " secondaryGrabbingObject.transform.position " + secondaryGrabbingObject.transform.position);
          //  Debug.Log(")
            
            var newScale = initialScale;
            newScale += new Vector3(newScaleX, newScaleY, newScaleZ);
            ApplyScale(newScale);
        }

        private void UniformScale()
        {
            float adjustedLength = (grabbedObject.transform.position - secondaryGrabbingObject.transform.position).magnitude;
            float adjustedScale = initialScaleFactor * adjustedLength;

            var newScale = new Vector3(adjustedScale, adjustedScale, adjustedScale);
            ApplyScale(newScale);
        }

        private float CalculateAxisScale(float centerPosition, float initialPosition, float currentPosition)
        {
            float distance = currentPosition - initialPosition;
            distance = (centerPosition < initialPosition ? distance : -distance);
            return distance;
        }


        private float calcRotation(float x, float y, float z, float angX, float angY, float angZ) {

        }
    }
}