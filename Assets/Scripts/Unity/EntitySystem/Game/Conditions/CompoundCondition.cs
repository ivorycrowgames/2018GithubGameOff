using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CompoundCondition : BaseCondition {

    public List<BaseCondition> conditions;
    public ConditionCombination operation = ConditionCombination.And;

    public override bool IsConditionPassed()
    {
        if (conditions.Count == 0)
        {
            return true;
        }

        if (operation == ConditionCombination.And)
        {
            foreach(BaseCondition con in conditions)
            {
                if (!con.IsConditionPassed())
                {
                    return false;
                }
            }

            return true;
        }
        else
        {
            foreach (BaseCondition con in conditions)
            {
                if (con.IsConditionPassed())
                {
                    return true;
                }
            }

            return false;
        }
    }
}
