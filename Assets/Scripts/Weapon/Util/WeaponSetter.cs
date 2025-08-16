using System;
using UnityEngine;

public class WeaponSetter : MonoBehaviour
{
    public WeaponCoreDataSO weaponCoreDatas;
    public AugmentDataSO parentData;
       
    public void SetWeapons()
    {
        var currentUpgradePoint = AugmentManager.Instance.GetAugmentedCount(parentData);
        for (int i = gameObject.transform.childCount - 1; i >= 0; i--)
        {
            ObjectPoolManager.Instance.Return(gameObject.transform.GetChild(i).gameObject);
        }
        
        float angleStep = 360f / currentUpgradePoint;  
        
        for (int i = 0; i < currentUpgradePoint; i++)
        {
            float angle = i * angleStep;  
            float radian = angle * Mathf.Deg2Rad;  
            
            float x = Mathf.Cos(radian) * weaponCoreDatas.attackRange;
            float y = Mathf.Sin(radian) * weaponCoreDatas.attackRange;
            
            Vector3 weaponPosition = transform.position + new Vector3(x, y, 0);

            GameObject weapon = ObjectPoolManager.Instance.Get(weaponCoreDatas.weaponPrefab, weaponPosition, new Vector3(0, 0, 0));
            
            weapon.transform.SetParent(transform);
            
            weapon.transform.rotation = Quaternion.Euler(0, 0, angle-90);
            
            weapon.name = $"Weapon_{i}_{angle}deg";
        }
    }
}