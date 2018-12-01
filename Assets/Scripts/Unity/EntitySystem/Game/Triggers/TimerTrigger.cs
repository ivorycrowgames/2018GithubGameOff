using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimerTrigger : BaseTrigger
{
    public float interval = 1.0f;
    public bool repeat = false;

    private float _remainingTime;

    void Start()
    {
        _remainingTime = interval;
        RegisterWithEntityManager(name);    
    }

    void Update()
    {
        if (_remainingTime <= 0)
        {
            FireTrigger();
            if (repeat)
            {
                _remainingTime = interval;
            }
        }
        else
        {
            _remainingTime -= Time.deltaTime;
        }
    }
}