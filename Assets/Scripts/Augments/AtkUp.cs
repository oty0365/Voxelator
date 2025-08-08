using UnityEngine;

public class AtkUp : AAugment,IPoolingObject
{
    public AugmentedDatasSO augmentedDatasSO;
    public override void Execute()
    {
        PlayerStatus.Instance.SetAtk(PlayerStatus.Instance.playerAtk.Value + Extracter.Instance.ParseFloat(augmentedDatasSO.datas[0]));
        ObjectPooler.Instance.Return(gameObject);
    }

    public void OnBirth()
    {
        Execute();
    }

    public void OnDeathInit()
    {
    }
}
