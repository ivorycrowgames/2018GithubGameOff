using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace IvoryCrow.Unity
{
    public class CenterOnCamera : MonoBehaviour
    {

        public CameraInfo cameraInfo;

        // Use this for initialization
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {
            transform.position = cameraInfo.Center;
        }
    }
}

