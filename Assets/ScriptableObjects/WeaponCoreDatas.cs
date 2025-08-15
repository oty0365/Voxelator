using UnityEngine;

[CreateAssetMenu(fileName = "WeaponCoreDatas", menuName = "Scriptable Objects/WeaponCoreDatas")]
public class WeaponCoreDatas : ScriptableObject
{
    public GameObject weaponPrefab;
    public float attackRange;
    public float rotationSpeed;
}
