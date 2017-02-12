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
      GameObject groupUtil = GameObject.Find("groupUtil");
      GameObject grp1 = GameObject.Find("grp1");
      GameObject sp1 = GameObject.Find("sp1");
      print("u was pressed");
      groupUtil.GetComponent<GroupUtil>().unGroupObject(sp1);
    }
  }
}
