using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using VRTK;
using System.Linq;
using ModVR;

public class GroupUtil : MonoBehaviour {
    public GameObject mergeMultipleObjects(List<GameObject> objects)
    {
        GameObject newObj = new GameObject("Empty");
        newObj.AddComponent<MeshFilter>();
        newObj.AddComponent<MeshRenderer>();
        string tstamp = getTime();
        newObj.name = "mergedObj" + tstamp;
        // newObj.tag = "mergedObj";

        Mesh combinedMesh = combineMeshes(objects);
        newObj.GetComponent<MeshFilter>().mesh = combinedMesh;

        //cleanup old object
        foreach (GameObject obj in objects)
        {
            Destroy(obj);
        }
        //setting the texture
        var texture = new Texture2D(2, 2, TextureFormat.ARGB32, false);
        texture.Apply();
        
        // connect texture to material of GameObject this script is attached to
        // GetComponent<Renderer>().material.mainTexture = texture;
        newObj.GetComponent<Renderer>().material.mainTexture = texture;
        // MeshCollider combinedMeshCollider = newObj.AddComponent<MeshCollider>();
        // combinedMeshCollider.sharedMesh = combinedMesh;
        newObj.AddComponent<BoxCollider>();
        Debug.Log(newObj.name);
        return newObj;
    }
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
        // newObj.tag = "mergedObj";

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
        Color c1 = obj1.GetComponent<Renderer>().material.color;
        Color c2 = obj2.GetComponent<Renderer>().material.color;
        newObj.GetComponent<Renderer>().material.color = ModVR_ColorUtil.combineTwoColors(c1,c2);
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
        // newObj.tag = "groupObj";
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
        CombineInstance[] combine = new CombineInstance[objects.Count];
        int i = 0;
        while (i < objects.Count)
        {
            MeshFilter meshFilter = objects[i].gameObject.GetComponent<MeshFilter>();
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
    public void autoWeld (Mesh mesh, float threshold, float bucketStep) {
        Vector3[] oldVertices = mesh.vertices;
        Vector3[] newVertices = new Vector3[oldVertices.Length];
        int[] old2new = new int[oldVertices.Length];
        int newSize = 0;
    
        // Find AABB
        Vector3 min = new Vector3 (float.MaxValue, float.MaxValue, float.MaxValue);
        Vector3 max = new Vector3 (float.MinValue, float.MinValue, float.MinValue);
        for (int i = 0; i < oldVertices.Length; i++) {
            if (oldVertices[i].x < min.x) min.x = oldVertices[i].x;
            if (oldVertices[i].y < min.y) min.y = oldVertices[i].y;
            if (oldVertices[i].z < min.z) min.z = oldVertices[i].z;
            if (oldVertices[i].x > max.x) max.x = oldVertices[i].x;
            if (oldVertices[i].y > max.y) max.y = oldVertices[i].y;
            if (oldVertices[i].z > max.z) max.z = oldVertices[i].z;
        }
    
        // Make cubic buckets, each with dimensions "bucketStep"
        int bucketSizeX = Mathf.FloorToInt ((max.x - min.x) / bucketStep) + 1;
        int bucketSizeY = Mathf.FloorToInt ((max.y - min.y) / bucketStep) + 1;
        int bucketSizeZ = Mathf.FloorToInt ((max.z - min.z) / bucketStep) + 1;
        List<int>[,,] buckets = new List<int>[bucketSizeX, bucketSizeY, bucketSizeZ];
    
        // Make new vertices
        for (int i = 0; i < oldVertices.Length; i++) {
            // Determine which bucket it belongs to
            int x = Mathf.FloorToInt ((oldVertices[i].x - min.x) / bucketStep);
            int y = Mathf.FloorToInt ((oldVertices[i].y - min.y) / bucketStep);
            int z = Mathf.FloorToInt ((oldVertices[i].z - min.z) / bucketStep);
        
            // Check to see if it's already been added
            if (buckets[x, y, z] == null)
                buckets[x, y, z] = new List<int> (); // Make buckets lazily
        
            for (int j = 0; j < buckets[x, y, z].Count; j++) {
                Vector3 to = newVertices[buckets[x, y, z][j]] - oldVertices[i];
                if (Vector3.SqrMagnitude (to) < threshold) {
                old2new[i] = buckets[x, y, z][j];
                goto skip; // Skip to next old vertex if this one is already there
                }
            }
    
        // Add new vertex
            newVertices[newSize] = oldVertices[i];
            buckets[x, y, z].Add (newSize);
            old2new[i] = newSize;
            newSize++;
        
            skip:;
        }
    
        // Make new triangles
        int[] oldTris = mesh.triangles;
        int[] newTris = new int[oldTris.Length];
        for (int i = 0; i < oldTris.Length; i++) {
            newTris[i] = old2new[oldTris[i]];
        }
        
        Vector3[] finalVertices = new Vector3[newSize];
        for (int i = 0; i < newSize; i++)
            finalVertices[i] = newVertices[i];
    
        mesh.Clear();
        mesh.vertices = finalVertices;
        mesh.triangles = newTris;
        mesh.RecalculateNormals ();
        ;
    }

    private void CleanupSelectedObject(GameObject interactableObj)
    {
        GameManager.instance.selectedObjectList = GameManager.instance.selectedObjectList.Where(o => o.name != interactableObj.name).ToList();
        interactableObj.GetComponent<ModVR_SelectHighlighter>().Unhighlight();
    }
}
