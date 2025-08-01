using UnityEngine;

public class DefUp : MonoBehaviour,IAugment,IPoolingObject
{
    [SerializeField, TextableAugment] private float amount;
    public void Execute()
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
