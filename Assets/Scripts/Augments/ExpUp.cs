using UnityEngine;

public class ExpUp : MonoBehaviour,IAugment,IPoolingObject
{
    [SerializeField,TextableAugment] private int amount;
    public void Execute()
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
