using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class GroupUtil : MonoBehaviour {
	public GameObject mergeObjects(GameObject obj1, GameObject obj2){
		List<GameObject> meshObjectList = new List<GameObject>();
	    meshObjectList.Add(obj1);
	    meshObjectList.Add(obj2);
	    GameObject newObj = new GameObject();
	    CombineInstance[] combine = new CombineInstance[meshObjectList.Count];
	    int i = 0;
	    while (i < meshObjectList.Count) {
	      MeshFilter meshFilter = meshObjectList[i].gameObject.GetComponent<MeshFilter>();
	      combine[i].mesh = meshFilter.sharedMesh;
	      combine[i].transform = meshFilter.transform.localToWorldMatrix;
	      i++;
	    }
	    Mesh combinedMesh= new Mesh();
	    combinedMesh.CombineMeshes(combine);
	    newObj.GetComponent<MeshFilter>().mesh = combinedMesh;
	    //cleanup old object
	    foreach (GameObject obj in meshObjectList){
	      Destroy(obj);
	    }
	    return newObj;
  }

  public GameObject mergeGroups(List<GameObject> objects, HashSet<List<string>> allCollisions){
    GameObject newObj = new GameObject();
    // List<GameObject> allChild = extractAllObjects(objects);
    // HashSet<List<int>> collisionSet = HashSet<List<int>>();
		// for (int i = 0; i < allCollisions.Count; i++) {
		// 	collisionSet.Add(Tuple.Create<int>(allCollisions[i].gameObject.GetInstanceID(), collisionObjects[i].GetInstanceID()));
    // }
    for (int i = 0; i < objects.Count; i++) {
      for (int j = i+1; i < objects.Count; j++) {
        List<string> pair = new List<string>();
        pair.Add(objects[i].name);
        pair.Add(objects[j].name);
			if (allCollisions.Contains(pair)) {
				mergeObjects (objects[i], objects[j]);
        }
      }
    }
    return newObj;
  }

  public GameObject groupObjects(List<GameObject> objects){
		GameObject newObj = new GameObject();
    // List<GameObject> allChild = extractAllObjects(objects);
    foreach (GameObject obj in objects){
	    obj.transform.parent = newObj.transform;
    }
    return newObj;
  }

  public GameObject unGroupObject(List<GameObject> parent, GameObject child){
    GameObject newObj = new GameObject();
    // List<GameObject> allChild = extractAllObjects(parent);
    foreach (GameObject obj in parent){
      if (obj != child){
        obj.transform.parent = newObj.transform;
      }
    }
    return newObj;
  }
  //
  // private void isColliding(GameObject obj1, GameObject obj2){
  //   bool collides = false;
  //
  //   return collides;
  // }

  public List<GameObject> extractAllObjects(List<GameObject> parents){
    List<GameObject> allChild = new List<GameObject>();
    foreach (GameObject obj in parents){
      foreach(Transform child in obj.transform){
				// allChild.Add(findObjByID(child.getInsanceID()));
        allChild.Add(child.gameObject);
      }
    }
    return allChild;
  }

  public GameObject findObjByID(int id){
    GameObject[] all = GameObject.FindGameObjectsWithTag("Untagged");
    for (int i = 0; i < all.Length; i++){
      if (all[i].GetInstanceID() == id){
        return all[i];
      }
    }
    return null;
  }
}
