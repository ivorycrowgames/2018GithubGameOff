using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using IvoryCrow.Unity;

public class GameStateEntity : BaseEntity
{
    public GameStateManager stateManager;

    protected override void OnStart()
    {
        AddActionHandler<StateChangeActionData>(handleStateAction);
    }

    protected override void OnFixedUpdate() { }

    private void handleStateAction(StateChangeActionData actionData)
    {
        stateManager.CurrentGameState = actionData.TargetState;
    }
}
