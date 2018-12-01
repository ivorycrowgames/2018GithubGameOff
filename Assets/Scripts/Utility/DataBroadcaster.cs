using IvoryCrow.Extensions;

using System;
using System.Collections;
using System.Collections.Generic;

namespace IvoryCrow.Utilities
{
    public class DataBroadcaster<T>
    {
        public delegate void Listener(T value);
        private List<Listener> _registeredListeners = new List<Listener>();

        public DataBroadcaster()
        {
        }

        public void AddListener(Listener listener)
        {
            if (!_registeredListeners.Contains(listener))
            {
                _registeredListeners.Add(listener);
            }
        }

        public void RemoveListener(Listener listener)
        {
            _registeredListeners.Remove(listener);
        }

        public void Broadcast(T broadcastData)
        {
            foreach(Listener listener in _registeredListeners)
            {
                listener(broadcastData);
            }
        }
    }
}
