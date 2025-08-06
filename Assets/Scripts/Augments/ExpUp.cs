using UnityEngine;

public class ExpUp : AAugment,IPoolingObject
{
    [SerializeField,TextableAugment] private int amount;
    public override void Execute()
    {
        PlayerStatus.Instance.SetExp(PlayerStatus.Instance.PlayerExp+amount);
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
