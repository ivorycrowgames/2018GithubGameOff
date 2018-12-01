using System;
using UnityEngine;

[AttributeUsage(AttributeTargets.Field, AllowMultiple = true)]
public class ObjectAsName : PropertyAttribute
{
    private Type _Type;
    public Type type
    {
        get { return _Type; }
    }

    public ObjectAsName(Type type)
    {
        _Type = type;
    }

}