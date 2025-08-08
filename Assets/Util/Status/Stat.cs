using System;

public class Stat<T>
{
    public event Action<T> OnChanged;
    protected T _value;

    public virtual T Value
    {
        get => _value;
        set
        {
            if (!Equals(_value, value))
            {
                _value = value;
                OnChanged?.Invoke(_value);
            }
        }
    }
}
