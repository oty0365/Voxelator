using UnityEngine;

public class FullHp : MonoBehaviour,IAugment,IPoolingObject
{
    public void Execute()
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
