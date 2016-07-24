using UnityEngine;
using System.Collections;

public class WandController : MonoBehaviour
{



    private SteamVR_TrackedObject trackedObj;
    private Valve.VR.EVRButtonId menuButton = Valve.VR.EVRButtonId.k_EButton_ApplicationMenu;
    private Valve.VR.EVRButtonId triggerButton = Valve.VR.EVRButtonId.k_EButton_SteamVR_Trigger;

    GameObject x;
    GameObject y;
    GameObject z;

    private GameObject selected;

    // Use this for initialization
    void Start()
    {
        trackedObj = GetComponent<SteamVR_TrackedObject>();

    }

    // Update is called once per frame
    void Update()
    {
        SteamVR_Controller.Device controller = SteamVR_Controller.Input((int)trackedObj.index);

        if (controller == null)
        {
            Debug.Log("Controller not initialized");
            return;
        }

        if (controller.GetPressDown(triggerButton) && selected != null)
        {

            Debug.Log("1st case");
            if(selected.name == "x")
            {
                float a = selected.transform.position.x;
                selected.transform.position = this.transform.position.x; 
            }
                    
                  


            //selected.transform.parent = this.transform;
           // selected.GetComponent<Rigidbody>().isKinematic = true;
        }
        if (controller.GetPressUp(triggerButton) && selected != null)
        {
            Debug.Log("2nd case");
           // selected.transform.parent = null;
            //selected.GetComponent<Rigidbody>().isKinematic = false;
        }
    }

    void OnTriggerEnter(Collider collider)
    {
        selected = collider.gameObject;
        x = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        x.name = "x"; 
        y = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        y.name = "y";
        z = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        z.name = "z";
        Vector3 positionx = new Vector3(1,0,0);
        x.transform.position = positionx;
        Vector3 positiony = new Vector3(0, 1, 0);
        y.transform.position = positiony;
        Vector3 positionz = new Vector3(0, 0, 1);
        z.transform.position = positionz;
    }

    void OnTriggerExit(Collider collider)
    {
        DestroyObject( x);
        DestroyObject(y);
        DestroyObject(z);
        selected = null;
    }
}
