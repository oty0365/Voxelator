using UnityEngine;

public class Machine : Type,ITypeCalculater
{
    public float Calculate(DamageType damageType,float value)
    {
        var damage = 0f;
        switch (damageType)
        {
            case DamageType.OverHeat:
                damage=value * 1.5f;
                break;
            case DamageType.Debug:
                damage = value * 1.5f;
                break;
            case DamageType.Virus:
                damage = value * 0.5f;
                break;
            case DamageType.Freeze:
                damage = 0;
                break;
        }
        typeBars[damageType].Value += damage;
        return damage;
    }
}
