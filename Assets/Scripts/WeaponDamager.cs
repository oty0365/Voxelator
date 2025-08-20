using UnityEngine;

public class WeaponDamager : Damager
{
    [SerializeField] private AugmentedDatasSO augmentedDatas;
    [SerializeField] private int parseIndex;

    public override DamageData GetDamage(float originalDamage)
    {
        var damageData = new DamageData();
        damageData.damage = originalDamage+damagerData.damage+Extracter.Instance.ParseFloat(augmentedDatas.datas[parseIndex]);;
        damageData.time = damagerData.infiniteTime;
        damageData.effectType = damagerData.effectType;
        damageData.effectDamage = damagerData.effectDamage;
        return damageData;
    }
}
