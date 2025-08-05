using UnityEngine;

public class DashMaster : MonoBehaviour,IAugment,IPoolingObject
{
    [TextableAugment, SerializeField] private float cooldown;
    public void Execute()
    {
        PlayerController.Instance.playerMovementData.runtimePlayerMovementData.maxDashCount++;
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
