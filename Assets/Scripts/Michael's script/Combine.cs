using UnityEngine;
using System.Collections;

[RequireComponent(typeof(MeshFilter))]
[RequireComponent(typeof(MeshRenderer))]

public class Combine : MonoBehaviour {

    // Use this for initialization
    GameObject Cube1;
    GameObject Cube2;
    GameObject Cube3;
    void Start () {
        Cube1 = GameObject.CreatePrimitive(PrimitiveType.Cube);
        Cube2 = GameObject.CreatePrimitive(PrimitiveType.Cube);
        Cube3 = GameObject.CreatePrimitive(PrimitiveType.Cube);
        Cube2.transform.position = new Vector3(1, 1, 1);
        Cube3.transform.position = new Vector3(2, 2, 2);
        MeshFilter[] meshFilters = new MeshFilter[3];
        meshFilters[0] = Cube1.GetComponent<MeshFilter>();
        meshFilters[1] = Cube2.GetComponent<MeshFilter>();
        meshFilters[2] = Cube3.GetComponent<MeshFilter>();
        CombineInstance[] combine = new CombineInstance[meshFilters.Length];
        int i = 0;
        while (i < meshFilters.Length)
        {
            combine[i].mesh = meshFilters[i].sharedMesh;
            combine[i].transform = meshFilters[i].transform.localToWorldMatrix;
            // meshFilters[i].gameObject.active = false;
            i++;
        }
        //Tube.transform.GetComponent<MeshFilter>().mesh = new Mesh();
        GameObject ac = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        ac.transform.GetComponent<MeshFilter>().mesh.CombineMeshes(combine);
        Cube2.GetComponent<MeshFilter>().mesh = ac.GetComponent<MeshFilter>().mesh;
        Destroy(ac);


    }
	
	// Update is called once per frame
	void Update () {
	
	}
}
