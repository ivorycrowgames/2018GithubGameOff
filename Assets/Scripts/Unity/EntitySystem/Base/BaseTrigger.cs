using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseTrigger : MonoBehaviour {
    public delegate void triggeredDelegate(string sourceTrigger);
    public triggeredDelegate OnTrigger;

    private string _triggerName;

    protected void RegisterWithEntityManager(string name)
    {
        _triggerName = name;
        EntityManager.Instance.AddTrigger(this);
    }

    public string GetTriggerName()
    {
        return _triggerName;
    }

    protected void FireTrigger()
    {
        if (OnTrigger != null)
        {
            OnTrigger(_triggerName);
        }
    }
}
