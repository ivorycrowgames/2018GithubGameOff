using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct CheckpointData
{
    public GameObject RespawnPosition;
}

public class CheckpointAction : SimpleGameAction<CheckpointData> { }
