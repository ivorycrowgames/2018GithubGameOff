using System;
using UnityEngine;

[System.Serializable]
public struct MoveActionData
{
    public Vector2 Direction;
    public float Speed;
    public float Distance;
    public bool ReturnToStart;
    public float HoldTime;
};

public class MoveAction : SimpleGameAction<MoveActionData>
{
}
