using System;
using System.Collections.Generic;
using UnityEngine;

public class LimitedStat : Stat<float>
{
    public new event Action<float, float> OnChanged;
    private float _maxValue;
    private Dictionary<BuffType, float> _buffs = new() { {BuffType.Add,0}, { BuffType.Mul ,1} };
    private Dictionary<BuffType, float> _maxBuffs = new() { {BuffType.Add,0}, { BuffType.Mul ,1} };
    
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

    public void SetBuff(BuffType type, float value)
    {
         _buffs[type] += value;
        Value = _buffs[BuffType.Mul]*_buffs[BuffType.Add];
    }

    public void SetMaxBuff(BuffType type, float value)
    {
         _maxBuffs[type] += value;
         MaxValue = _maxBuffs[BuffType.Mul]*_maxBuffs[BuffType.Add];
    }
}