using System;
using System.Collections.Generic;

public class UnlimitedStat : Stat<float>
{
    public new event Action<float> OnChanged;
    private Dictionary<BuffType, float> _buffs = new() { {BuffType.Add,0}, { BuffType.Mul ,1} };
    
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
    public void SetBuff(BuffType type, float value)
    {
        _buffs[type] += value;
        Value = _buffs[BuffType.Mul]*_buffs[BuffType.Add];
    }
}
