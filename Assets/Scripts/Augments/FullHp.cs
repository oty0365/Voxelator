using UnityEngine;

public class FullHp : AAugment,IPoolingObject
{
    public override void Execute()
    {
        PlayerStatus.Instance.SetHp(PlayerStatus.Instance.PlayerMaxHp);
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
