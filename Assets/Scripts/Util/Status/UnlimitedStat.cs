using System;

public class UnlimitedStat : Stat<float>
{
    public new event Action<float> OnChanged;
    public override float Value
    {
        get => _value;
        set
        {
            if (value != _value)
            {
                _value = value;
            }
            OnChanged?.Invoke(_value);
        }
    }
}
