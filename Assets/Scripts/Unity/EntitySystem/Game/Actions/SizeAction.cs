using System;
using System.Collections.Generic;
using UnityEngine;

using IvoryCrow.Utilities;

[System.Serializable]
public struct MoveSide
{
    public Direction Direction; 
    public float Distance;
};

[System.Serializable]
public struct SizeActionData
{
    public float Speed;
    public List<MoveSide> SidesToMove;
};

public class SizeAction : SimpleGameAction<SizeActionData>
{
}
