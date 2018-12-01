using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct CameraFollowActionData
{
    public GameObject followEntity;
    public float transitionTime;
};

public class CameraFollowAction : SimpleGameAction<CameraFollowActionData> { }
