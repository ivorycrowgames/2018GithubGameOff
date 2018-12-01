using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProxyTrigger : BaseTrigger
{
    // Use this for initialization
    void Start()
    {
        RegisterWithEntityManager(this.name);
    }

    public void Fire()
    {
        FireTrigger();
    }
}
