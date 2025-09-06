using UnityEngine;

public class Monster: Type,ITypeCalculater
{
    public float Calculate(EffectType damageType,float value)
    {
        var damage = 0f;
        switch (damageType)
        {
            case EffectType.Freeze:
                damage=value * 1.5f;
                break;
            case EffectType.Virus:
                damage = value * 1.2f;
                break;
            case EffectType.Debug:
                damage = value * 0.5f;
                break;
            case EffectType.OverHeat:
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
