using System;

public enum BuffType
{
    Add,
    Mul,
    Set,
}

public class Stat<T>
{
    public event Action<T> OnChanged;
    protected T _value;
    protected T _baseVal;

    public virtual T BaseVal
    {
        get=>_baseVal;
        set
        {
            if (!Equals(_value, value))
            {
                _value = value;
            }
        }
    }

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
