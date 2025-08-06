using UnityEngine;

public class AtkUp : AAugment,IPoolingObject
{
    [SerializeField,TextableAugment] private float amount;
    public override void Execute()
    {
        PlayerStatus.Instance.SetAtk(PlayerStatus.Instance.PlayerAtk+amount);
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
