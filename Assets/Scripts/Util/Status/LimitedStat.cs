using System;
using System.Collections.Generic;
using UnityEngine;

public class LimitedStat : Stat<float>
{
    public new event Action<float, float> OnChanged;
    private float _maxValue;
    private Dictionary<BuffType, float> _buffs = new() { {BuffType.Add,0}, { BuffType.Mul ,1} };
    private Dictionary<BuffType, float> _maxBuffs = new() { {BuffType.Add,0}, { BuffType.Mul ,1} };
    private float _baseMaxVal;
    
    
    public float MaxValue
    {
        get => _maxValue;
        set
        {
            if (value != _maxValue) 
            {
                _maxValue = value;
            }
            OnChanged?.Invoke(_value, _maxValue);
        }
    }

    public override float Value
    {
        get => _value;
        set
        {
            var clamped = Mathf.Clamp(value, 0, _maxValue);
            if (!Mathf.Approximately(clamped, _value))
            {
                _value = clamped;
            }
            OnChanged?.Invoke(_value, _maxValue);
        }
    }
    public override float BaseVal
    {
        get=>_baseVal;
        set
        {
            if (!Equals(_baseVal, value))
            {
                _baseVal = value;
                Value = _buffs[BuffType.Mul]*(_buffs[BuffType.Add]+BaseVal);
            }
        }
    }

    public float BaseMaxVal
    {
        get => _baseMaxVal;
        set
        {
            if (!Equals(_baseMaxVal, value))
            {
                _baseMaxVal = value;
                MaxValue = _maxBuffs[BuffType.Mul]*(_maxBuffs[BuffType.Add]+BaseMaxVal);
            }
        }
    }

    public void AddBuff(BuffType type, float value)
    {
        _buffs[type] += value;
        Value = _buffs[BuffType.Mul] * (_buffs[BuffType.Add] + BaseVal);
    }

    public void SetBuff(BuffType type, float value)
    {
        _buffs[type]=value;
        Value = _buffs[type]*(_buffs[BuffType.Mul] + BaseMaxVal);
    }

    public void AddMaxBuff(BuffType type, float value)
    {
         _maxBuffs[type] += value;
         MaxValue = _maxBuffs[BuffType.Mul]*(_maxBuffs[BuffType.Add]+BaseMaxVal);
    }

    public void SetMaxBuff(BuffType type, float value)
    {
        _maxBuffs[type]=value;
        MaxValue = _maxBuffs[BuffType.Mul]*(_maxBuffs[BuffType.Add]+BaseMaxVal);
    }
    
}