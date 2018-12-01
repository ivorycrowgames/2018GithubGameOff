using IvoryCrow.Extensions;
using System;

namespace IvoryCrow.Utilities
{
    public class ValueChangeDetector<T>
    {
        public delegate void ValueChanged(T oldValue, T newValue);
        public ValueChanged OnValueChanged;

        private T _value;
        public T Value {
            get { return _value; }
            set
            {
                if (!value.Equals(_value))
                {
                    T oldValue = _value;
                    _value = value;
                    
                    if (OnValueChanged != null)
                    {
                        OnValueChanged(oldValue, _value);
                    }
                }
            }
        }

        public ValueChangeDetector(): this(default(T))
        {
        }

        public ValueChangeDetector(T initialValue)
        {
            _value = initialValue;
        }
    }
}
