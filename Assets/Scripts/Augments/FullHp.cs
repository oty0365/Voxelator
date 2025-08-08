using UnityEngine;

public class FullHp : AAugment,IPoolingObject
{
    public override void Execute()
    {
        PlayerStatus.Instance.SetHp(PlayerStatus.Instance.playerHp.MaxValue);
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
