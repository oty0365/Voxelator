using System.Collections.Generic;
using UnityEngine;

public abstract class Type
{
    public GameObject owner;
    public Dictionary<EffectType,LimitedStat> typeBars = new(){
        {EffectType.Debug , new LimitedStat{BaseMaxVal = 100,Value = 0}},
        {EffectType.Freeze, new LimitedStat{BaseMaxVal = 100,Value = 0}},
        {EffectType.OverHeat, new LimitedStat{BaseMaxVal = 100,Value = 0}},
        {EffectType.Virus ,new LimitedStat{BaseMaxVal = 100,Value = 0}}
    };
    public void Resistance(EffectType damageType)
    {
        typeBars[damageType].SetBuff(BuffType.Add,-typeBars[damageType].BaseMaxVal);
        typeBars[damageType].BaseMaxVal += 50;
    }

    public void Check()
    {
        foreach (var typeBar in typeBars)
        {
            if (typeBar.Value.Value >= typeBar.Value.MaxValue)
            {
                Resistance(typeBar.Key);
                EffectComputeManager.Instance.RunEffect(typeBar.Key,owner);
            }
        }
    }
}
