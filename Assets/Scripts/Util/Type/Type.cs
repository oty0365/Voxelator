using System.Collections.Generic;
using UnityEngine;

public abstract class Type
{
    public Dictionary<DamageType,LimitedStat> typeBars = new(){
        {DamageType.Debug , new LimitedStat{MaxValue = 100,Value = 0}},
        {DamageType.Freeze, new LimitedStat{MaxValue = 100,Value = 0}},
        {DamageType.OverHeat, new LimitedStat{MaxValue = 100,Value = 0}},
        {DamageType.Virus ,new LimitedStat{MaxValue = 100,Value = 0}}
    };
    public void Resistance(DamageType damageType)
    {
        typeBars[damageType].MaxValue += 50;
        typeBars[damageType].Value = 0;
    }
}
