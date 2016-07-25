using UnityEngine;
using System.Collections;

[RequireComponent(typeof(MeshFilter))]
[RequireComponent(typeof(MeshRenderer))]



public class Spheretotube : MonoBehaviour {

    public GameObject menu;

    private SteamVR_TrackedObject trackedObj;
    private Valve.VR.EVRButtonId menuButton = Valve.VR.EVRButtonId.k_EButton_ApplicationMenu;
    private Valve.VR.EVRButtonId triggerButton = Valve.VR.EVRButtonId.k_EButton_SteamVR_Trigger;

    private bool menuButtonDown;
    //private bool triggerButtonDown;
    //private bool triggerButtonUp;
    private bool showMenu;

    private GameObject selected;
    float a = 5;
    float b = 1;
    float r = 5;
    float c = 1; 

    // Use this for initialization
    private GameObject cube;
    GameObject sphere;

    GameObject Tube1;
    GameObject Tube;
    void Start () {

        trackedObj = GetComponent<SteamVR_TrackedObject>();
        menu.SetActive(false);
        menuButtonDown = false;
        showMenu = false;
        sphere = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        sphere.GetComponent<Renderer>().material.color.a.Equals(1);
        cube = GameObject.Find("/Sphere");
        Renderer rend = GetComponent<Renderer>();
        rend.material.shader = Shader.Find("Specular");
        rend.material.SetColor("_SpecColor", Color.red);
        Tube = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        Tube1 = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        Tube1.GetComponent<Renderer>().material.color.a.Equals(1);
        float e = a + (float)0.01;
        float f = Mathf.Sqrt(25 - Mathf.Pow(a, 2));
        float g = (a + b) / 2;
        Vector3 position = new Vector3(e,f,g);
        //Tube.transform.position = position;


    }

    // Update is called once per frame
    int count = 0;
    void Update() {
        SteamVR_Controller.Device controller = SteamVR_Controller.Input((int)trackedObj.index);

        if (controller == null)
        {
            Debug.Log("Controller not initialized");
            return;
        }

        menuButtonDown = controller.GetPressDown(menuButton);

        if (menuButtonDown)
        {
            showMenu = !showMenu;
            menu.SetActive(showMenu);
        }

        if (controller.GetPressDown(triggerButton) && selected != null)
        {
            Debug.Log("1st case");
            selected.transform.parent = this.transform;
            selected.GetComponent<Rigidbody>().isKinematic = true;
        }
        if (controller.GetPressUp(triggerButton) && selected != null)
        {
            Debug.Log("2nd case");
            selected.transform.parent = null;
            selected.GetComponent<Rigidbody>().isKinematic = false;
        }
        if (count < 1)
        {
 
                /*a = a + 0.01;
                b = Mathf.Sqrt(25 - Mathf.Pow(a, 2));
                c = (a + b) / 2;
                
                selected
                a = a + (float)0.1;
                Vector3 position = new Vector3(a, b, c);*/
                sphere.transform.position = selected.transform.position;
                Debug.Log( sphere.transform.position);

            Vector3[] TubeVerts = Tube.GetComponent<MeshFilter>().mesh.vertices;
            Debug.Log(TubeVerts.Length);





            //Vector3[] SphereVerts = sphere.GetComponent<MeshFilter>().mesh;
            //ArrayList<Vector3> TubeVertsList = new ArrayList<Vector3>(TubeVerts.Concat<Vector3>(SphereVerts)); 
            // TubeVerts.add( sphere.GetComponent<MeshFilter>().mesh.vertices);
            //var TubeVertsList = new Vector3[TubeVerts.Length + SphereVerts.Length];
            // TubeVerts.CopyTo(TubeVertsList, 0);
            // SphereVerts.CopyTo(TubeVertsList, TubeVerts.Length);
            // Tube.GetComponent<MeshFilter>().mesh.vertices = TubeVertsList;

            MeshFilter[] meshFilters = new MeshFilter[2];
            meshFilters[0] = Tube.GetComponent<MeshFilter>();
            meshFilters[1] = sphere.GetComponent<MeshFilter>();
            CombineInstance[] combine = new CombineInstance[meshFilters.Length];
            int i = 0;
            while (i < meshFilters.Length)
            {
                combine[i].mesh = meshFilters[i].sharedMesh;
                combine[i].transform = meshFilters[i].transform.localToWorldMatrix;
                //Tube.transform.gameObject.active = false;
                i++;
            }
            // GameObject ac = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            Tube1.transform.GetComponent<MeshFilter>().mesh = new Mesh();

            Tube1.transform.GetComponent<MeshFilter>().mesh.CombineMeshes(combine);
            Tube.GetComponent<MeshFilter>().mesh = Tube1.GetComponent<MeshFilter>().mesh;
            Tube.name = "Tube";

            // Tube.transform.gameObject.active = true;
            count = 0;
        }


       
    

    }
    void OnTriggerEnter(Collider collider)
    {
        selected = collider.gameObject;
    }

    void OnTriggerExit(Collider collider)
    {
        selected = null;
    }
}
