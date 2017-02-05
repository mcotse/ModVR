using UnityEngine;
using System.Collections;



public class CubeBender : MonoBehaviour {
    private GameObject cube;
    float increment = 0;
    // Use this for initialization

    void Start () {

        cube = GameObject.Find("/Cube");
        Renderer rend = GetComponent<Renderer>();
        rend.material.shader = Shader.Find("Specular");
        rend.material.SetColor("_SpecColor", Color.red);




        //cube4.transform.position = (MinP + MaxP) / 2; 

    }
	
	// Update is called once per frame
	void Update () {

        Mesh mesh = GetComponent<MeshFilter>().mesh;
        Vector3[] verts = mesh.vertices;
        Vector3[] MinMax = minMaxCalc(verts);
        Vector3 MinP = MinMax[0];
        Vector3 MaxP = MinMax[1];
        Vector3 MidP = (MinP + MaxP) / 2;
        /*print(MaxP);
        print(MaxX);
        print(MinP);
        print(MinX);
        print((MinP + MaxP) / 2);*/

        GameObject cube2 = GameObject.CreatePrimitive(PrimitiveType.Cube);
        GameObject cube3 = GameObject.CreatePrimitive(PrimitiveType.Cube);

        increment += (float)0.1;
        MidP[1] += increment;
		
        print(MidP);
        print(MinP);

        Vector3 CurrenScale = cube.transform.lossyScale;
        Vector3 CurrentRotation = cube.transform.eulerAngles;

        CurrenScale.z = CalculateLength(MinP, MidP);
        CurrentRotation.x = CurrentRotation.x - CalculateRotation((MidP - MinP), (MaxP - MinP));
        //Cube.transform.
        cube2.transform.position = (MidP + MinP) / 2;
        cube2.transform.Rotate(CurrentRotation);
        cube2.transform.localScale = CurrenScale;

        //Cube.transform.
        CurrentRotation = cube.transform.eulerAngles;
        CurrenScale = cube.transform.lossyScale;
        CurrenScale.z = CalculateLength(MidP, MaxP);
        CurrentRotation.x = CurrentRotation.x + CalculateRotation((MaxP - MidP), (MaxP - MinP));
        cube3.transform.position = (MidP + MaxP) / 2;
        cube3.transform.Rotate(CurrentRotation);
        cube3.transform.localScale = CurrenScale;


    }


    public Vector3[] minMaxCalc(Vector3[] verts) {
        Vector3 MaxP = new Vector3(0, 0, 0);
        Vector3 MinP = new Vector3(0, 0, 0);
        float MaxX = Mathf.NegativeInfinity;
        float MinX = Mathf.Infinity;

        Vector3 WorldMax;
        Vector3 LocalMax;
        Vector3 WorldMin;
        Vector3 LocalMin;
        for (int i = 0; i < verts.LongLength; i++)
        {
            WorldMax = cube.transform.TransformPoint(verts[i]);
            LocalMax = transform.TransformPoint(WorldMax);
            WorldMin = WorldMax;
            LocalMin = transform.TransformPoint(WorldMin);
            if (LocalMax.z > MaxX)
            {
                MaxX = LocalMax.z;
                MaxP = WorldMax;
            }
            if (LocalMin.z < MinX)
            {
                MinX = LocalMin.z;
                MinP = WorldMin;
            }
        }
        Vector3[] MinMax = new Vector3[] { MinP, MaxP };

        return MinMax; 
    }

    public float CalculateRotation(Vector3 a, Vector3 b)
    {
        float Rotation;

        float c = a.x * b.x + a.y * b.y + a.z * b.z;
        float d = Mathf.Sqrt(a.x * a.x + a.y * a.y + a.z * a.z);
        float e = Mathf.Sqrt(b.x * b.x + b.y * b.y + b.z * b.z); 
        Rotation = Mathf.Acos(c / (d * e) )/(2*Mathf.PI)*360;
        print(c / (d * e)); 
        print(Rotation);
        return Rotation;

    }
    public float CalculateLength(Vector3 a, Vector3 b) {
        float Length= Mathf.Sqrt(Mathf.Pow((b.x-a.x),2)+Mathf.Pow((b.y - a.y), 2)+ Mathf.Pow((b.z - a.z), 2));
        return Length;
    }

}

