using UnityEngine;

public enum EffectType
{
    None,
    OverHeat,
    Virus,
    Freeze,
    Debug
}
public struct DamageData {
    public float damage;
    public float time;
    public EffectType effectType;
    public float effectDamage;
}


public interface IDamageEffector
{
    public void GiveEffect(ref GameObject target);
}

public interface IDamager
{
    public DamageData GetDamage(float originalDamage);
}
public class Damager : MonoBehaviour,IDamager
{
    [SerializeField] DamagerDataSO damagerData;
    public GameObject parent;
    
    public DamageData GetDamage(float originalDamage)
    {
        var damageData = new DamageData();
        damageData.damage = originalDamage+damagerData.damage;
        damageData.time = damagerData.infiniteTime;
        damageData.effectType = damagerData.effectType;
        damageData.effectDamage = damagerData.effectDamage;
        return damageData;
    }
}
