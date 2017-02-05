using UnityEngine;
 using System.Collections;

 public class CollisionPassing : MonoBehaviour {

     public GameObject recGO;

    //  SteamVR_Controller.Device controllerLeft;
     SteamVR_Controller.Device controllerRight;
    //  controllerLeft = SteamVR_Controller.Input(SteamVR_Controller.GetDeviceIndex(SteamVR_Controller.DeviceRelation.Leftmost));
     controllerRight = SteamVR_Controller.Input(SteamVR_Controller.GetDeviceIndex(SteamVR_Controller.DeviceRelation.Rightmost));

     public void OnCollisionEnter(Collision col)
     {
         controllerRight.GetComponent<MergeController>().ReceiveCollision(ref col);
     }
 }
