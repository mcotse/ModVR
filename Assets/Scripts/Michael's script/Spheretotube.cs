using UnityEngine;
using System.Collections;

[RequireComponent(typeof(MeshFilter))]
[RequireComponent(typeof(MeshRenderer))]

public class Spheretotube : MonoBehaviour {
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
	void Update () {
        if (count < 1)
        {
            if (a < 1000 && b < 1000 && c < 1000)
            {
                /*a = a + 0.01;
                b = Mathf.Sqrt(25 - Mathf.Pow(a, 2));
                c = (a + b) / 2;
                */
                a = a + (float)0.1;
                Vector3 position = new Vector3(a, b, c);
                sphere.transform.position = position;
                Debug.Log(position);

            }
            else
            {
                a = 1;
               // b = 2;
                //c = 10;
            }
            count++; 
        }
        else {
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

}
