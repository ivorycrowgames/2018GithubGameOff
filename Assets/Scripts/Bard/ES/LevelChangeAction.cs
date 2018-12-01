using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct LevelChangeData
{
    public int LevelBuildIndex;
    public bool UpdateSaveFile;
}

public class LevelChangeAction : SimpleGameAction<LevelChangeData> { }
