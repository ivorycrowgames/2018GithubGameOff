using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EntityManager : Singleton<EntityManager> {

    private Dictionary<string, List<ConditionalSequence>> _triggerToSequence = new Dictionary<string, List<ConditionalSequence>>();
    private Dictionary<string, BaseTrigger> _triggerMap = new Dictionary<string, BaseTrigger>();
    private Dictionary<string, BaseEntity> _entityMap = new Dictionary<string, BaseEntity>();

    private static object _lock = new object();
    private bool registeredSceneHandler = false;

    protected class ConditionalSequence
    {
        public BaseCondition Condition;
        public List<SequenceAction> Actions;

        public ConditionalSequence()
        {
            Condition = null;
            Actions = new List<SequenceAction>();
        }
    };

    protected EntityManager()
    {
    }

    void Awake()
    {
        lock (_lock)
        {
            if (!registeredSceneHandler)
            {
                SceneManager.sceneLoaded += OnSceneLoaded;
                registeredSceneHandler = true;
            }
        }
    }

    // called second
    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        Reset();
    }

    // called when the game is terminated
    void OnDisable()
    {
        lock (_lock)
        {
            if (registeredSceneHandler)
            {
                SceneManager.sceneLoaded -= OnSceneLoaded;
                registeredSceneHandler = false;
            }
        }
    }

    public void AddTrigger(BaseTrigger trigger)
    {
        string triggerName = trigger.GetTriggerName();
        if (_triggerMap.ContainsKey(triggerName))  
        {
            throw new System.Exception("Cannot add trigger named: " + triggerName + " one already exists");
        }

        _triggerMap.Add(triggerName, trigger);
        trigger.OnTrigger += handleTrigger;
    }

    public void AddEntity(BaseEntity entity)
    {
        string entityName = entity.GetEntityName();
        if (_entityMap.ContainsKey(entityName))
        {
            throw new System.Exception("Cannot add entity named: " + entityName + " one already exists");
        }

        _entityMap.Add(entityName, entity);
    }

    public void AddSequence(EntitySequence sequence)
    {
        foreach(string trigger in sequence.Triggers)
        {
            if (!_triggerToSequence.ContainsKey(trigger))
            {
                _triggerToSequence.Add(trigger, new List<ConditionalSequence>());
            }

            var seq = new ConditionalSequence();
            seq.Condition = sequence.Condition;
            seq.Actions.AddRange(sequence.Actions);
            _triggerToSequence[trigger].Add(seq);
        }
    }

    public void Reset()
    {
        _triggerToSequence.Clear();
        _triggerMap.Clear();
        _entityMap.Clear();
    }

    private void handleTrigger(string sourceTrigger)
    {
        List<ConditionalSequence> sequenceList;
        bool hasActions = _triggerToSequence.TryGetValue(sourceTrigger, out sequenceList);
        if (hasActions)
        {
            foreach(ConditionalSequence sequence in sequenceList)
            {
                // Check sequence condition if present
                if (sequence.Condition != null)
                {
                    if (!sequence.Condition.IsConditionPassed())
                    {
                        continue;
                    }
                }

                foreach(SequenceAction action in sequence.Actions)
                {
                    BaseEntity targetEntity;
                    bool validTarget = _entityMap.TryGetValue(action.TargetEntity, out targetEntity);
                    if (!validTarget)
                    {
                        Debug.Log("Attemped to perform action on entity which doesn't exist: " + action.TargetEntity);
                        continue;
                    }

                    if (action.DelayTime == 0)
                    {
                        targetEntity.HandleAction(action.Action.ActionData(), action.Action.ActionType());
                    }
                    else
                    {
                        StartCoroutine(executeAfterTime(action.DelayTime, action.Action, targetEntity));
                    }
                }
            }
            
        }
    }

    IEnumerator executeAfterTime(float time, BaseGameAction action, BaseEntity target)
    {
        yield return new WaitForSeconds(time);
        if (target && action)
        {
            target.HandleAction(action.ActionData(), action.ActionType());
        }
        
    }
}
