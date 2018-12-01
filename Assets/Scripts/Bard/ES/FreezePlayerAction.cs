using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct FreezePlayerActionData
{
    public bool isFrozen;
};

public class FreezePlayerAction : SimpleGameAction<FreezePlayerActionData> { }
