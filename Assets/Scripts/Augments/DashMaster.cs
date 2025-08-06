using UnityEngine;

public class DashMaster : AAugment,IPoolingObject
{
    [TextableAugment, SerializeField] private float cooldown;
    public override void Execute()
    {
        PlayerController.Instance.playerMovementData.runtimePlayerMovementData.maxDashCount++;
        UpLoad();
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
