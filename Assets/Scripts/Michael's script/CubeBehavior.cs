using UnityEngine;
using System.Collections;

public class CubeBehavior : MonoBehaviour {
    //bool flag = true;
    // Use this for initialization
    GameObject particle; 


    void Start () {
        Debug.Log("I am alive!");
        transform.position = new Vector3(0, 5, 0);
        GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Cube);

        //Cube.transform.
        cube.transform.position = new Vector3(2, 3, 0);

        /*       if (flag) {

              }
              flag = false;

      
    BoxCollider rb = GetComponent<BoxCollider>();
        rb.transform.position = new Vector3(0, 0, 0);*/
    }

    // Update is called once per frame
    double x = 0, y = 0, z = 0;
    void Update () {
        
        if (x < 10) {
            x += 0.1;
            y += 0.1;
        }
        else
        {
            x -= 10;
            y -= 10;
        }
        transform.position = new Vector3((float)x, (float)y, (float)z); 
        Vector3 startingPoint;
        Vector3 endPoint;
        foreach (Touch touch in Input.touches)
        {
            if (touch.phase == TouchPhase.Began)
            {
                var ray = Camera.main.ScreenPointToRay(touch.position);
                if (Physics.Raycast(ray))
                {
                    // Create a particle if hit
                    Instantiate(particle, transform.position, transform.rotation);
                }
               startingPoint = transform.position;


            }
            
            else if (touch.phase == TouchPhase.Stationary) {
                //set end point
                var ray = Camera.main.ScreenPointToRay(touch.position);
                if (Physics.Raycast(ray))
                {
                    // Create a particle if hit
                    Instantiate(particle, transform.position, transform.rotation);
                }
                endPoint = transform.position;
                //falg ended 
                // here we should get the curser position after the starting point has been set to show a less opaque line 
                //of where it would be 

                //sudo
               /* if (started && ended) {
                    GameObject line = GameObject.CreatePrimitive(PrimitiveType.Cube);
                    line.transform.localScale = new Vector3(0.1, 0.1, length);
                    //calculate rotation, calculate length, and translate in the middle. 


                }*/


            }

        }


        
    }
}


