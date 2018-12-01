using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SequenceAction
{
    public BaseGameAction Action;
    public float DelayTime = 0;

    [ObjectAsName(typeof(BaseEntity))]
    public string TargetEntity = "";
}

public class EntitySequence : MonoBehaviour {

    public BaseCondition Condition;

    [ObjectAsName(typeof(BaseTrigger))]
    public List<string> Triggers = new List<string>();

    public List<SequenceAction> Actions = new List<SequenceAction>();

    private void Start()
    {
        EntityManager.Instance.AddSequence(this);
    }
}
