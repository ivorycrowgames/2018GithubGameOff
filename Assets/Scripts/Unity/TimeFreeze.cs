using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace IvoryCrow.Unity
{
    public class TimeFreeze : MonoBehaviour
    {
        private void Awake()
        {
        }
 
        private void OnEnable()
        {
            Time.timeScale = 0;
        }

        private void OnDisable()
        {
            Time.timeScale = 1;
        }
    }
}

