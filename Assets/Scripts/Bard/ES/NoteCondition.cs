using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoteCondition : BaseCondition
{
    public BardController entity;
    public int greaterThanOrEqualTo;

    public override bool IsConditionPassed()
    {
        return entity.collectedNotes >= greaterThanOrEqualTo;
    }
}
