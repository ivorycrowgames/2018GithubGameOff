using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct CameraLookActionData
{
    public GameObject lookTarget;
    public float transitionTime;
};

public class CameraLookAction : SimpleGameAction<CameraLookActionData> { }
