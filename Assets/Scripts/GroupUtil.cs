using UnityEngine;
using System.Collections;

public class GroupUtil : MonoBehaviour {
  public GameObject mergeObjects(List<GameObject> meshObjectList){
    // List<GameObject> meshObjectList;
    // meshObjectList.Add(obj1);
    // meshObjectList.Add(obj2);
    GameObject newObj;
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

  public GameObject mergeGroups(List<GameObject> objects, List<Collision> allCollisions, List<GameObject> collisionObjects){
    GameObject newObj;
    List<GameObject> allChild = extractAllObjects(objects);
    HashSet collisionSet;
    for (int i = 0; i < allCollision.Count; i++) {
      collisionSet.Add(Tuple<int,int>(allCollision[i].gameObject.GetInstanceID, collisionObjects[i].GetInstanceID));
    }
    for (int i = 0; i < allChild.Count; i++) {
      for (int j = i+1; i < allChild.Count; j++) {
        if (collisionSet.Contains(Tuple<int,int>(allChild[i].GetInstanceID,allChild[j].GetInstanceID))) {
          mergeObjects(allChild[i],allChild[j])
        }
      }
    }
    return newObj;
  }

  public GameObject groupObjects(List<GameObject> objects){
    GameObject newObj;
    List<GameObject> allChild = extractAllObjects(objects);
    foreach (GameObject obj in allChild){
	    child.transform.parent = newObj.transform;
    }
    return newObj;
  }

  public int unGroupObject(int parent, int child){
    GameObject newObj;
    List<GameObject> allChild = extractAllObjects(objects);
    foreach (GameObject obj in allChild){
      if (obj != child){
        obj.transform.parent = newObj.transform;
      }
    }
    return newObj;
  }

  private void isColliding(GameObject obj1, GameObject obj2){
    bool collides = false;

    return collides;
  }

  private List<GameObject> extractAllObjects(List<GameObject> parents){
    List<GameObject> allChild;
    foreach (GameObject obj in objects){
      foreach(Transform child in obj.transform){
		    allChild.Add(child)
      }
    }
    return allChild;
  }
}
