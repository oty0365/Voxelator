using UnityEngine;

public class HpUp : AAugment,IPoolingObject
{
    [SerializeField] private AugmentedDatasSO augmentedDatasSO;
    public override void Execute()
    {
        PlayerStatus.Instance.SetMaxHp(PlayerStatus.Instance.playerHp.MaxValue + Extracter.Instance.ParseFloat(augmentedDatasSO.datas[0]));
        PlayerStatus.Instance.SetHp(PlayerStatus.Instance.playerHp.Value + Extracter.Instance.ParseFloat(augmentedDatasSO.datas[0]));
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
