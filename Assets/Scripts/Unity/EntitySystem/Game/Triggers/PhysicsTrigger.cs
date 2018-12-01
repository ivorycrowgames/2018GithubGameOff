using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhysicsTrigger : BaseTrigger {
    public bool FireOnce = false;
    public bool TriggerOnExit = false;
    public string TagFilter = "";

    private List<GameObject> _currentlyTriggeringObjects = new List<GameObject>();
    private bool _hasFired = false;

    void Start()
    {
        RegisterWithEntityManager(this.name);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (_currentlyTriggeringObjects.Contains(other.gameObject))
        {
            return;
        }

        _currentlyTriggeringObjects.Add(other.gameObject);
        if (!TriggerOnExit)
        {
            fireIfTagsMatch(other);
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (_currentlyTriggeringObjects.Contains(other.gameObject))
        {
            _currentlyTriggeringObjects.Remove(other.gameObject);
        }

        if (TriggerOnExit)
        {
            fireIfTagsMatch(other);
        }
    }

    private void OnDisable()
    {
        _currentlyTriggeringObjects.Clear();
    }

    private void fireIfTagsMatch(Collider2D other)
    {
        if (_hasFired && FireOnce)
        {
            return;
        }
   
        if (TagFilter.Length != 0)
        {
            if (other.CompareTag(TagFilter))
            {
                this.FireTrigger();
                _hasFired = true;
            }
        }
        else
        {
            this.FireTrigger();
            _hasFired = true;
        }
    }
}
