using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyTrigger : BaseTrigger {

    public KeyCode triggerKey;

	// Use this for initialization
	void Start () {
        RegisterWithEntityManager(this.name);
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown(triggerKey))
        {
            FireTrigger();
        }
	}
}
