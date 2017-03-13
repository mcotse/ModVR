using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace ModVR
{
    public class ModVR_PlayerObject : MonoBehaviour
    {
        public enum ObjectTypes
        {
            Null,
            Highlighter,
            Selector
        }

        public ObjectTypes objectType;

        public static void SetPlayerObject(GameObject obj, ObjectTypes objType)
        {
            if (!obj.GetComponent<ModVR_PlayerObject>())
            {
                var playerObject = obj.AddComponent<ModVR_PlayerObject>();
                playerObject.objectType = objType;
            }
        }

        public static bool IsPlayerObject(GameObject obj, ObjectTypes ofType = ObjectTypes.Null)
        {
            foreach (var playerObject in obj.GetComponentsInParent<ModVR_PlayerObject>(true))
            {
                if (ofType == ObjectTypes.Null || ofType == playerObject.objectType)
                {
                    return true;
                }
            }
            return false;
        }
    }
}
