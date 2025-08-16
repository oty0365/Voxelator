using UnityEngine;

public class LongSwordAugment : AAugment,IPoolingObject
{
    public static GameObject core;
    [SerializeField] private WeaponCoreDataSO weaponCoreDatas;
    public void OnBirth()
    {
        Execute();
    }
    public void OnDeathInit() 
    {
        
    }
    public override void Execute() 
    {
        if (core == null)
        {
            core = ObjectPoolManager.Instance.Get(ObjectBankManager.Instance.Get("WeaponCore"), PlayerInput.Instance.gameObject.transform.position, new Vector3(0, 0, 0));
            var weaponCore = core.GetComponent<WeaponCore>();
            var weaponSetter = core.transform.GetChild(0).gameObject.GetComponent<WeaponSetter>();
            weaponCore.weaponCoreDatas = weaponCoreDatas;
            weaponSetter.weaponCoreDatas = weaponCoreDatas;
            weaponSetter.parentData = keyData;
        }
        UpLoad();
        core.GetComponent<WeaponCore>().Equip();
        ObjectPoolManager.Instance.Return(gameObject);
    }
}
