using UnityEngine;

public class DefUp : AAugment,IPoolingObject
{
    [SerializeField, TextableAugment] private float amount;
    public override void Execute()
    {
        PlayerStatus.Instance.SetDef(PlayerStatus.Instance.PlayerDef+amount);
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
