using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using VRTK;
using System.Linq;

public class GroupUtil : MonoBehaviour {
	public GameObject mergeObjects(GameObject obj1, GameObject obj2, bool destoryOld){
		Debug.Log("in mergeObjects");
		Debug.Log(obj1.name);
		Debug.Log(obj2.name);
		List<GameObject> meshObjectList = new List<GameObject>();
    meshObjectList.Add(obj1);
    meshObjectList.Add(obj2);
    GameObject newObj = new GameObject("Empty");
		newObj.AddComponent<MeshFilter>();
		newObj.AddComponent<MeshRenderer>();
		string tstamp = getTime();
    newObj.name = "mergedObj" + tstamp;
		newObj.tag = "mergedObj";
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
		if (destoryOld){
			foreach (GameObject obj in meshObjectList){
				Destroy(obj);
			}
		}
		Debug.Log(newObj.name);
    return newObj;
  }

    public GameObject mergeGroups(List<VRTK_InteractableObject> objects, List<List<string>> allCollisions)
    {
        GameObject newObj = new GameObject();
        // List<GameObject> allChild = extractAllObjects(objects);
        // HashSet<List<int>> collisionSet = HashSet<List<int>>();
        // for (int i = 0; i < allCollisions.Count; i++) {
        // 	collisionSet.Add(Tuple.Create<int>(allCollisions[i].gameObject.GetInstanceID(), collisionObjects[i].GetInstanceID()));
        // }

        HashSet<int> toRemove = new HashSet<int>();
        Debug.Log("in mergeGroups");
        for (int i = 0; i < objects.Count; i++)
        {
            for (int j = i + 1; j < objects.Count; j++)
            {
                List<string> pair = new List<string>();
                pair.Add(objects[i].name);
                pair.Add(objects[j].name);
                pair.Sort();
                for (int m = 0; m < allCollisions.Count; m++)
                {
                    allCollisions[m].Sort();
                    if (Enumerable.SequenceEqual(allCollisions[m], pair))
                    {
                        mergeObjects(objects[i].gameObject, objects[j].gameObject, false);
                        toRemove.Add(i);
                        toRemove.Add(j);
                    }
                }
            }
        }
        for (int i = objects.Count - 1; i > -1; i--)
        {
            if (toRemove.Contains(i))
            {
                Destroy(objects[i]);
                objects.RemoveAt(i);
            }
        }
        foreach (VRTK_InteractableObject obj in objects)
        {
            obj.transform.parent = newObj.transform;
        }
        return newObj;
    }

  public GameObject groupObjects(List<VRTK_InteractableObject> objects){
		GameObject newObj = new GameObject();
		string tstamp = Guid.NewGuid().ToString();
		newObj.name = "groupObj" + tstamp;
		newObj.tag = "groupObj";
    foreach (VRTK_InteractableObject obj in objects){
	    obj.transform.parent = newObj.transform;
    }
    return newObj;
  }

  public GameObject unGroupObject(GameObject child){
		child.transform.parent = null;
		return child;
  }

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
	public string getTime(){
		return System.DateTime.Now.ToString("hhss", System.Globalization.CultureInfo.InvariantCulture);
	}
}
