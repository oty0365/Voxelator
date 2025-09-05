using UnityEngine;

[CreateAssetMenu(fileName = "DamagerDataSO", menuName = "Scriptable Objects/DamagerDataSO")]
public class DamagerDataSO : ScriptableObject
{
    public float damage;
    public float infiniteTime;
    public EffectType effectType;
    public float effectDamage;
}
