using UnityEngine;

public class AtkUp : MonoBehaviour,IAugment,IPoolingObject
{
    [SerializeField,TextableAugment] private float amount;
    public void Execute()
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
