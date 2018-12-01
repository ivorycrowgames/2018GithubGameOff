using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public abstract class SimpleCondition<EntityType, ValueType> : BaseCondition
{
    public EntityType entity;
    public ConditionState operation = ConditionState.Equal;
    public ValueType value;

    public delegate ValueType ValueExtractor(EntityType ent);
    private ValueExtractor _extractor = null;

    public override bool IsConditionPassed()
    {
        bool isEqual = _extractor(entity).Equals(value);
        if (operation == ConditionState.NotEqual)
        {
            return !isEqual;
        }

        return isEqual;
    }

    protected void SetValueExtractor(ValueExtractor extractor)
    {
        _extractor = extractor;
    }
}
