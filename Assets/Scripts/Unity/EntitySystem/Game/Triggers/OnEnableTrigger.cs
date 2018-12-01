using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnEnableTrigger : BaseTrigger
{
    private bool _hasRegistered = false;
    private int _frameDelay = 1;
    private int _remaingingFrameDelay;

    void Start()
    {
        _remaingingFrameDelay = _frameDelay;
    }

    void Update()
    {
        if (!_hasRegistered && _remaingingFrameDelay == 0)
        {
            RegisterWithEntityManager(name);
            FireTrigger();
            _hasRegistered = true;
        }
        else
        {
            _remaingingFrameDelay--;
        }
    }
}