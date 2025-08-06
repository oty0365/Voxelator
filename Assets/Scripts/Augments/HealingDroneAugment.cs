using UnityEngine;

public class HealingDroneAugment : AAugment,IPoolingObject
{
    [SerializeField] private GameObject healingDrone;
    public override void Execute()
    {
        ObjectPooler.Instance.Get(healingDrone, PlayerStatus.Instance.gameObject.transform.position,new Vector3(0,0,0));
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
