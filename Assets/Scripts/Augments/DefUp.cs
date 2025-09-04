using UnityEngine;

public class DefUp : AAugment,IPoolingObject
{
    [SerializeField] AugmentedDatasSO augmentedDatasSO;
    public override void Execute()
    {
        PlayerStatus.Instance.playerDef.SetBuff(BuffType.Add,Extracter.Instance.ParseFloat(augmentedDatasSO.datas[0]));
        ObjectPoolManager.Instance.Return(gameObject);
    }

    public void OnBirth()
    {
        Execute();
    }

    public void OnDeathInit()
    {
    }
}
