using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ConditionState {
    Equal,
    NotEqual
}

public enum ConditionCombination
{
    And,
    Or
}

public abstract class BaseCondition : MonoBehaviour
{
    public abstract bool IsConditionPassed();
}
