using UnityEngine;
using System.Collections;
using System.Collections.Generic;

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
      groupUtil.GetComponent<GroupUtil>().mergeObjects(cube12,cube13);

    }
    if (Input.GetKeyDown("g")){
      GameObject groupUtil = GameObject.Find("groupUtil");
      GameObject cube12 = GameObject.Find("cube12");
      GameObject cube13 = GameObject.Find("cube13");
      List<GameObject> objects = new List<GameObject>();
      print("g was pressed");
      objects.Add(cube12);
      objects.Add(cube13);
      groupUtil.GetComponent<GroupUtil>().groupObjects(objects);
    }
    if (Input.GetKeyDown("u")){
      GameObject groupUtil = GameObject.Find("groupUtil");
      GameObject cube12 = GameObject.Find("cube12");
      GameObject cube13 = GameObject.Find("cube13");
      List<GameObject> objects = new List<GameObject>();
      print("u was pressed");
      objects.Add(cube12);
      objects.Add(cube13);
      groupUtil.GetComponent<GroupUtil>().groupObjects(objects);
    }
  }
}
