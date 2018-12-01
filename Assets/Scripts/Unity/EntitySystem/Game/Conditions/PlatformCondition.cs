using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformCondition : BaseCondition
{
    public bool Not;
    public RuntimePlatform Platform;

    public override bool IsConditionPassed()
    {
        if (Not)
        {
            return Application.platform != Platform;
        }

        return Application.platform == Platform;
    }
}
