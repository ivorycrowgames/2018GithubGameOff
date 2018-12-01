using System;
using UnityEngine;

using IvoryCrow.Unity;

[System.Serializable]
public struct StateChangeActionData
{
    public GameState TargetState;
};

public class StateChangeAction : SimpleGameAction<StateChangeActionData>
{
}
