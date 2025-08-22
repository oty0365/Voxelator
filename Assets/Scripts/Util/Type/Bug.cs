using UnityEngine;

public class Bug: Type,ITypeCalculater
{
    public float Calculate(DamageType damageType,float value)
    {
        var damage = 0f;
        switch (damageType)
        {
            case DamageType.Debug:
                damage=value * 2f;
                break;
            case DamageType.Freeze:
                damage = value * 1.2f;
                break;
            case DamageType.OverHeat:
                damage = value * 0.5f;
                break;
            case DamageType.Virus:
                damage = 0;
                break;
        }
        typeBars[damageType].Value += damage;
        return damage;
    }
}
