using UnityEngine;

public class Monster: Type,ITypeCalculater
{
    public float Calculate(DamageType damageType,float value)
    {
        var damage = 0f;
        switch (damageType)
        {
            case DamageType.Freeze:
                damage=value * 1.5f;
                break;
            case DamageType.Virus:
                damage = value * 1.2f;
                break;
            case DamageType.Debug:
                damage = value * 0.5f;
                break;
            case DamageType.OverHeat:
                damage = 0;
                break;
        }
        typeBars[damageType].Value += damage;
        return damage;
    }
}
