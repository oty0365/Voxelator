using UnityEngine;

public class Machine : Type,ITypeCalculater
{
    public float Calculate(EffectType damageType,float value)
    {
        var damage = 0f;
        switch (damageType)
        {
            case EffectType.OverHeat:
                damage=value * 1.5f;
                break;
            case EffectType.Debug:
                damage = value * 1.5f;
                break;
            case EffectType.Virus:
                damage = value * 0.5f;
                break;
            case EffectType.Freeze:
                damage = 0;
                break;
        }
        typeBars[damageType].Value += damage;
        if (owner.GetComponent<EffectContainer>().HasEffect(damageType))
        {
            damage = 0;
        }
        return damage;
    }
}
