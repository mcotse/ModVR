
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System;

public class KeyPressTrigger : MonoBehaviour {
  void Update() {
    if (Input.GetKeyDown("space")){
      GameObject groupUtil = GameObject.Find("groupUtil");
      GameObject cube12 = GameObject.Find("cube12");
      GameObject cube13 = GameObject.Find("cube13");
      List<GameObject> objects = new List<GameObject>();
      print("space key was pressed");
      print("testing merge objects");
      // util.mergeObjects(cube12,cube13);
      objects.Add(cube12);
      objects.Add(cube13);
      groupUtil.GetComponent<GroupUtil>().mergeObjects(cube12,cube13,true);

    }
        if (Input.GetKeyDown("g"))
        {
            GameObject groupUtil = GameObject.Find("groupUtil");
            GameObject cube12 = GameObject.Find("cube12");
            GameObject cube13 = GameObject.Find("cube13");
            GameObject sp2 = GameObject.Find("sp2");
            GameObject sp1 = GameObject.Find("sp1");
            List<GameObject> objects = new List<GameObject>();
            print("g was pressed");
            objects.Add(cube12);
            objects.Add(cube13);
            objects.Add(sp2);
            objects.Add(sp1);
            groupUtil.GetComponent<GroupUtil>().groupObjects(GameManager.instance.interactableObjectList);
            //groupUtil.GetComponent<GroupUtil>().groupObjects(objects);
        }
        if (Input.GetKeyDown("u")){
      GameObject groupUtil = GameObject.Find("groupUtil");
      GameObject grp1 = GameObject.Find("grp1");
      GameObject sp1 = GameObject.Find("sp1");
      print("u was pressed");
      groupUtil.GetComponent<GroupUtil>().unGroupObject(sp1);
    }
    if (Input.GetKeyDown("m")){
      print("object being imported...");
      ModVR_ObjImporter.ImportGameObjectFile("cube1");
    }
    if (Input.GetKeyDown("s")){
      print("s was pressed");
      print("object being saved...");
      // GameObject objExporter = GameObject.Find("groupUtil");
      // ModVR_ObjExporter ObjExporter = new ModVR_ObjExporter();
      GameObject cube1 = GameObject.Find("cube1");
      ModVR_ObjExporter.GameObjectToFile(cube1);
      // MeshFilter cube1Mesh = (MeshFilter)cube1.GetComponent("MeshFilter");
      // objExporter.GetComponent<ObjectExporter>().MeshToFile(cube1Mesh,"testexport1");
      // ObjExporter.MeshToFile(cube1Mesh);
      print("object saved to: " + Directory.GetCurrentDirectory());
    }
     if (Input.GetKeyDown("p")){
      print("testing...");
      GameObject groupUtil = GameObject.Find("groupUtil");
      int i = 0;
      float j = 0;
      List<GameObject> objects = new List<GameObject>();
      System.Random rand = new System.Random();
      while(i < 100)
      {
        
        GameObject newObj = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        newObj.name = "i" + i.ToString();
        newObj.transform.localScale = new Vector3(0.05f, 0.05f, 0.05f);
        newObj.transform.position = new Vector3(j+(float)(rand.NextDouble()*0.02), j+(float)(rand.NextDouble()*0.02), j+(float)(rand.NextDouble()*0.2));
        i ++;
        j += 0.02f;
        objects.Add(newObj);
      }
      ModVR_ObjExporter.GameObjectToFile(GameObject.Find("i0"));
      GameObject merged = groupUtil.GetComponent<GroupUtil>().mergeMultipleObjects(objects);
      float threshold = 0.001f;

      // for (int m=1; m < 10;m++)
      // {
      //   GameObject mergedcopy = Instantiate(merged);  
      //   threshold *= 2;
      //   float bucketsize = 0.001f;
      //   for (int n=1; n < 10; n++)
      //   {
      //     bucketsize *= 2;
      //     groupUtil.GetComponent<GroupUtil>().autoWeld(mergedcopy.GetComponent<MeshFilter>().sharedMesh,threshold,bucketsize);
      //     Debug.Log("t: " + threshold.ToString() + ", b: " + bucketsize.ToString() + ", v: " + mergedcopy.GetComponent<MeshFilter>().mesh.vertices.Length.ToString());
      //     Destroy(mergedcopy);
      //   }
        
      // }
      groupUtil.GetComponent<GroupUtil>().autoWeld(merged.GetComponent<MeshFilter>().sharedMesh,0.004f,0.008f);
      Debug.Log("v: " + merged.GetComponent<MeshFilter>().mesh.vertices.Length.ToString());
      ModVR_ObjExporter.GameObjectToFile(merged);

    if (Input.GetKeyDown("c")){
      GameObject s1 = GameObject.Find("s1");
      GameObject c1 = GameObject.Find("c1");
      GameObject groupUtil = GameObject.Find("groupUtil");
      groupUtil.GetComponent<GroupUtil>().mergeObjects(s1,c1,true);
    }
    if (Input.GetKeyDown("x")){
      GameObject s1 = GameObject.Find("s1");
      GameObject c1 = GameObject.Find("c1");
      ModVR_ColorUtil.changeColor(s1,"white");
      ModVR_ColorUtil.changeColor(c1,"red");
    }
  }
}
