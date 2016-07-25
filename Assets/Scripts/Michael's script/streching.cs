using UnityEngine;
using System.Collections;

public class Stretching : MonoBehaviour
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
        float[] difference = new float[2]; 
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
                selected.transform.position = new Vector3 (this.transform.position.x, selected.transform.position.y, selected.transform.position.z);
                difference[0] = selected.transform.position.x - a;
                difference[1] = 0; 
            }
            if (selected.name == "y")
            {
                float a = selected.transform.position.y;
                selected.transform.position = new Vector3(selected.transform.position.x, this.transform.position.y, selected.transform.position.z);
                difference[0] = selected.transform.position.y - a;
                difference[1] = 1;
            }
            if (selected.name == "z")
            {
                float a = selected.transform.position.z;
                selected.transform.position = new Vector3(selected.transform.position.x, selected.transform.position.y, this.transform.position.z);
                difference[0] = selected.transform.position.z - a;
                difference[1] = 2;
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
        switch (difference[1].ToString()) {
            case "0":
                selected.transform.localScale = new Vector3(selected.transform.localScale.x*(1+difference[0]),selected.transform.localScale.y,selected.transform.localScale.z);
                break;
            case "1":
                selected.transform.localScale = new Vector3(selected.transform.localScale.x , selected.transform.localScale.y * (1 + difference[0]), selected.transform.localScale.z);
                break;
            case "2":
                selected.transform.localScale = new Vector3(selected.transform.localScale.x , selected.transform.localScale.y, selected.transform.localScale.z * (1 + difference[0]));
                break;

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
