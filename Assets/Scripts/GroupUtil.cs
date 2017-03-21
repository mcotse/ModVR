using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using VRTK;
using System.Linq;
using ModVR;

public class GroupUtil : MonoBehaviour {
    public GameObject mergeObjects(GameObject obj1, GameObject obj2, bool destoryOld)
    {
        List<GameObject> meshObjectList = new List<GameObject>();
        meshObjectList.Add(obj1);
        meshObjectList.Add(obj2);
        GameObject newObj = new GameObject("Empty");
        newObj.AddComponent<MeshFilter>();
        newObj.AddComponent<MeshRenderer>();
        string tstamp = getTime();
        newObj.name = "mergedObj" + tstamp;
        newObj.tag = "mergedObj";


        // CombineInstance[] combine = new CombineInstance[meshObjectList.Count];
        // int i = 0;
        // while (i < meshObjectList.Count)
        // {
        //     MeshFilter meshFilter = meshObjectList[i].gameObject.GetComponent<MeshFilter>();
        //     combine[i].mesh = meshFilter.sharedMesh;
        //     combine[i].transform = meshFilter.transform.localToWorldMatrix;
        //     i++;
        // }
        // Mesh combinedMesh = new Mesh();
        // combinedMesh.CombineMeshes(combine);
        Mesh combinedMesh = combineMeshes(meshObjectList);
        newObj.GetComponent<MeshFilter>().mesh = combinedMesh;
        //cleanup old object

        if (destoryOld)
        {
            foreach (GameObject obj in meshObjectList)
            {
                Destroy(obj);
            }
        }
        //setting the texture
        var texture = new Texture2D(2, 2, TextureFormat.ARGB32, false);
        texture.Apply();
        
        // connect texture to material of GameObject this script is attached to
        // GetComponent<Renderer>().material.mainTexture = texture;
        newObj.GetComponent<Renderer>().material.mainTexture = texture;


        MeshCollider combinedMeshCollider = newObj.AddComponent<MeshCollider>();
        combinedMeshCollider.sharedMesh = combinedMesh;
        Debug.Log(newObj.name);
        return newObj;
    }

    public GameObject mergeGroups(List<ModVR_InteractableObject> objects, List<List<string>> allCollisions)
    {
        GameObject newObj = new GameObject();
        List<GameObject> mergedObjs = new List<GameObject>();
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
                        GameObject merged = mergeObjects(objects[i].gameObject, objects[j].gameObject, false);
                        mergedObjs.Add(merged);
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
                CleanupSelectedObject(objects[i].gameObject);

                Destroy(objects[i].gameObject);
                objects.RemoveAt(i);
            }
        }
        foreach (ModVR_InteractableObject obj in objects)
        {
            obj.transform.parent = newObj.transform;
        }
        foreach (GameObject obj in mergedObjs)
        {
            obj.transform.parent = newObj.transform;
        }
        newObj.name = "mergeGroup";
        return newObj;
    }

    public GameObject groupObjects(List<ModVR_InteractableObject> objects)
    {
        GameObject newObj = new GameObject();
        string tstamp = Guid.NewGuid().ToString();
        newObj.name = "groupObj" + tstamp;
        newObj.tag = "groupObj";
        foreach (ModVR_InteractableObject obj in objects)
        {
            obj.transform.parent = newObj.transform;
        }
        List<GameObject> objectList = new List<GameObject>();
        foreach (ModVR_InteractableObject obj in objects)
        {
            objectList.Add(obj.gameObject);
        }
        // Mesh combinedMesh = new Mesh();
        Mesh combinedMesh = combineMeshes(objectList);
        MeshCollider combinedMeshCollider = newObj.AddComponent<MeshCollider>();
        combinedMeshCollider.sharedMesh = combinedMesh;
        //MeshCollider combinedMeshCollider = new MeshCollider();
        //combinedMeshCollider.sharedMesh = combinedMesh;
        //newObj.AddComponent(combinedMeshCollider);
        return newObj;
    }
    private Mesh combineMeshes(List<GameObject> objects)
    {
        List<GameObject> meshObjectList = new List<GameObject>();
        foreach (GameObject obj in objects)
        {
            meshObjectList.Add(obj);
        }
        CombineInstance[] combine = new CombineInstance[meshObjectList.Count];
        int i = 0;
        while (i < meshObjectList.Count)
        {
            MeshFilter meshFilter = meshObjectList[i].gameObject.GetComponent<MeshFilter>();
            combine[i].mesh = meshFilter.sharedMesh;
            combine[i].transform = meshFilter.transform.localToWorldMatrix;
            i++;
        }
        Mesh combinedMesh = new Mesh();
        combinedMesh.CombineMeshes(combine);
        return combinedMesh;
    }

    public GameObject unGroupObject(GameObject child)
    {
        child.transform.parent = null;
        return child;
    }

    public List<GameObject> extractAllObjects(List<GameObject> parents)
    {
        List<GameObject> allChild = new List<GameObject>();
        foreach (GameObject obj in parents)
        {
            foreach (Transform child in obj.transform)
            {
                // allChild.Add(findObjByID(child.getInsanceID()));
                allChild.Add(child.gameObject);
            }
        }
        return allChild;
    }

    public GameObject findObjByID(int id)
    {
        GameObject[] all = GameObject.FindGameObjectsWithTag("Untagged");
        for (int i = 0; i < all.Length; i++)
        {
            if (all[i].GetInstanceID() == id)
            {
                return all[i];
            }
        }
        return null;
    }

    public string getTime()
    {
        return System.DateTime.Now.ToString("hhss", System.Globalization.CultureInfo.InvariantCulture);
    }

    public List<GameObject> GetAllChildren(GameObject parent)
    {
        List<GameObject> children = new List<GameObject>();

        Transform transform = parent.transform;
        if (!(parent.name.Contains("group") || parent.name.Contains("merged")))
        {
            children.Add(parent);
        }

        foreach(Transform t in transform)
        {
            List<GameObject> groupMergedObjs = new List<GameObject>();
            string name = t.name;

            if (!name.Contains("Highlight"))
            {
                if (name.Contains("group") || name.Contains("merged"))
                {
                    children = children.Union(GetAllChildren(t.gameObject)).ToList();
                }
                else
                {
                    children.Add(t.gameObject);
                }
            }
        }

        return children;
    }

    private void CleanupSelectedObject(GameObject interactableObj)
    {
        GameManager.instance.selectedObjectList = GameManager.instance.selectedObjectList.Where(o => o.name != interactableObj.name).ToList();
        interactableObj.GetComponent<ModVR_SelectHighlighter>().Unhighlight();
    }
}
