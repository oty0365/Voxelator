using UnityEngine;

public class HpUp : AAugment,IPoolingObject
{
    [SerializeField, TextableAugment] private float amount;
    public override void Execute()
    {
        PlayerStatus.Instance.SetMaxHp(PlayerStatus.Instance.PlayerMaxHp+amount);
        PlayerStatus.Instance.SetHp(PlayerStatus.Instance.PlayerHp+amount);
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
