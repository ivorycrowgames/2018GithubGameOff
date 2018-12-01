using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class SimpleGameAction<T> : BaseGameAction
{
    public T data = default(T);

    public override object ActionData()
    {
        return data;
    }

    public override Type ActionType()
    {
        return typeof(T);
    }
}

public abstract class BaseGameAction : MonoBehaviour {
    public abstract object ActionData();
    public abstract Type ActionType();
}
