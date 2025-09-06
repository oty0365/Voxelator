using UnityEngine;

[CreateAssetMenu(fileName = "WeaponCoreDatas", menuName = "Scriptable Objects/WeaponCoreDatas")]
public class WeaponCoreDataSO : ScriptableObject
{
    public GameObject weaponPrefab;
    public float attackRange;
    public float rotationSpeed;
}
