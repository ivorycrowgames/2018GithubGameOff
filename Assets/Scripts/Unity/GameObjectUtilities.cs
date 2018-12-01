using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GameObjectUtilities {
    public static void GetComponentIfNull<T>(MonoBehaviour parent, ref T currentValue)
    {
        if (currentValue.Equals(default(T)))
        {
            currentValue = parent.GetComponent<T>();
        }
    }
}
