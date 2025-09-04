using UnityEngine;

public class FullHp : AAugment,IPoolingObject
{
    public override void Execute()
    {
        PlayerStatus.Instance.playerHp.Value = PlayerStatus.Instance.playerHp.MaxValue;
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
