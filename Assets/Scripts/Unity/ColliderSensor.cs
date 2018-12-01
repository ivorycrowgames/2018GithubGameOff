using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColliderSensor : MonoBehaviour {

    public bool IgnoreTriggers = true;

    public string[] layersToCollide;
    public string tagFilter;

    public delegate void OnTriggerAction(Collider2D other);
    public OnTriggerAction onTriggerEnter;
    public OnTriggerAction onTriggerExit;

    private List<Collider2D> contacts = new List<Collider2D>();

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void FixedUpdate () {
        contacts.RemoveAll((Collider2D col) => !col.gameObject.activeSelf);
	}

    void OnTriggerEnter2D(Collider2D other)
    {
        if (!doesPassFilter(other))
        {
            return;
        }

        if (!contacts.Contains(other))
        {
            contacts.Add(other);
        }

        if (onTriggerEnter != null)
        {
            onTriggerEnter(other);
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (!doesPassFilter(other))
        {
            return;
        }

        contacts.Remove(other);
        
        if (onTriggerExit != null)
        {
            onTriggerExit(other);
        }
    }

    private bool doesPassFilter(Collider2D other)
    {
        if (IgnoreTriggers && other.isTrigger)
        {
            return false;
        }

        bool passesTag = true;
        if (tagFilter.Length > 0)
        {
            if (!other.gameObject.tag.Equals(tagFilter))
            {
                passesTag = false;
            }
        }

        if (layersToCollide.Length > 0)
        {
            bool passesFilter = false;
            foreach (string layer in layersToCollide)
            {
                if (other.gameObject.layer == LayerMask.NameToLayer(layer))
                {
                    passesFilter = true;
                    break;
                }
            }

            return passesFilter && passesTag;
        }

        return passesTag;
    }

    public int CurrentTouchingEntitiesCount()
    {
        return contacts.Count;
    }

    public List<Collider2D> CurrentTouchingEntities()
    {
        return new List<Collider2D>(contacts);
    }
}
