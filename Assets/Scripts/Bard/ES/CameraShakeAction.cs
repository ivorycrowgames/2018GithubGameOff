using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct CameraShakeActionData
{
    public float magnitude;
    public float duration;
    public int shakeCount;
};

public class CameraShakeAction : SimpleGameAction<CameraShakeActionData> { }
