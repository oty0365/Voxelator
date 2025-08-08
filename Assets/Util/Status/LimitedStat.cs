using System;
using UnityEngine;

public class LimitedStat : Stat<float>
{
    public new event Action<float, float> OnChanged;

    private float _maxValue;

    public float MaxValue
    {
        get => _maxValue;
        set
        {
            if (value != _maxValue) 
            {
                _maxValue = value;
                Value = _maxValue;
            }
        }
    }

    public override float Value
    {
        get => _value;
        set
        {
            if (value != _value)
            {
                _value = Mathf.Clamp(value, 0, _maxValue);
            }
            OnChanged?.Invoke(_value, _maxValue);
        }
    }
}