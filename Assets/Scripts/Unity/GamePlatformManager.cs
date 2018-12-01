using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using IvoryCrow.Constants;

namespace IvoryCrow.Unity
{
    [System.Serializable]
    public class PlatformSpecificGameObject
    {
        public bool Desktop;
        public bool Android;
        public GameObject Object;
    }

    public class GamePlatformManager : MonoBehaviour
    {
        public List<PlatformSpecificGameObject> PlatformSpecificGameObjects;

        // Use this for initialization

        // Update is called once per frame
        void Start()
        {
            foreach (PlatformSpecificGameObject obj in PlatformSpecificGameObjects)
            {
                if (Application.platform == RuntimePlatform.Android)
                {
                    obj.Object.SetActive(obj.Android);
                }
                else
                {
                    obj.Object.SetActive(obj.Desktop);
                }
            }
        }
    }
}
