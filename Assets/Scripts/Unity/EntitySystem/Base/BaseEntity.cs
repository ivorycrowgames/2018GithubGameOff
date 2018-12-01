using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseEntity : MonoBehaviour {

    public delegate void ActionHandler<T>(T item);
    private delegate void InnerActionHandler(object action);
    private Dictionary<Type, InnerActionHandler> _actionHandler = new Dictionary<Type, InnerActionHandler>();

    private string _entityName;
    private void RegisterWithEntityManager(string name)
    {
        _entityName = name;
        AddActionHandler<DisableActionData>((DisableActionData a) => { gameObject.SetActive(false); });
        AddActionHandler<EnableActionData>((EnableActionData a) => { gameObject.SetActive(true); });
        EntityManager.Instance.AddEntity(this);
    }

    public void Start()
    {
        RegisterWithEntityManager(name);
        OnStart();
    }
    protected abstract void OnStart();

    public void FixedUpdate()
    {
        OnFixedUpdate();
    }
    protected abstract void OnFixedUpdate();

    public string GetEntityName()
    {
        return _entityName;
    }

    public void HandleAction<T>(T actionData)
    {
        this.HandleAction(actionData, typeof(T));
    }

    public void HandleAction(object actionData, Type actionDataType)
    {
        InnerActionHandler handler;
        var haveHandler = _actionHandler.TryGetValue(actionDataType, out handler);
        if (!haveHandler)
        {
            Debug.Log("No handler for action type " + actionDataType.ToString() + " present on entity: " + GetEntityName());
            return;
        }

        handler(actionData);
    }

    protected void AddActionHandler<T>(ActionHandler<T> handler) where T : struct
    {
        Type t = typeof(T);
        if (_actionHandler.ContainsKey(t))
        {
            throw new System.Exception("Cannot add type handler: " + GetEntityName() + "->" + t.ToString() + " already exists");
        }

        _actionHandler.Add(t, (object actionData) =>
        {
            var typedGameAction = (T) actionData;
            handler(typedGameAction);
        });
    }
}
