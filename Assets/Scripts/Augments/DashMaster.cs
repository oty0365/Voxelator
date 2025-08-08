using UnityEngine;

public class DashMaster : AAugment,IPoolingObject
{
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
