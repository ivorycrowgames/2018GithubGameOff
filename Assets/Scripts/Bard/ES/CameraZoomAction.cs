using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct CameraZoomActionData
{
    public float transitionTime;
    public float orthographicSize;
};

public class CameraZoomAction : SimpleGameAction<CameraZoomActionData> { }
