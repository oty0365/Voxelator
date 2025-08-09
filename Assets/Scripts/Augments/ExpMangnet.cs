using UnityEngine;

public class ExpMangnet : AAugment,IPoolingObject
{
    public override void Execute()
    {
        ExpGiver.magnetToPlayer = true;
        UpLoad();
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
