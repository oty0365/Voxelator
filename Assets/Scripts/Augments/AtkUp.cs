using UnityEngine;

public class AtkUp : AAugment,IPoolingObject
{
    public AugmentedDatasSO augmentedDatasSO;
    public override void Execute()
    {
        PlayerStatus.Instance.playerAtk.AddBuff(BuffType.Add,Extracter.Instance.ParseFloat(augmentedDatasSO.datas[0]));
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
