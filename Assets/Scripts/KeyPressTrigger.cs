
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;

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
