using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct ShowEnvTextActionData
{
    public string Text;
};

public class ShowEnvTextAction : SimpleGameAction<ShowEnvTextActionData> { }
