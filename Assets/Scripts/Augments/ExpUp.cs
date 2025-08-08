using UnityEngine;

public class ExpUp : AAugment,IPoolingObject
{
    [SerializeField] AugmentedDatasSO augmentedDatasSO;
    public override void Execute()
    {
        PlayerStatus.Instance.SetExp(PlayerStatus.Instance.PlayerExp + Extracter.Instance.ParseFloat(augmentedDatasSO.datas[0]));
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
