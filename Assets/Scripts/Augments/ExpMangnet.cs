using UnityEngine;

public class ExpMangnet : MonoBehaviour,IAugment,IPoolingObject
{
    [SerializeField] private AugmentData thisData;
    public void Execute()
    {
        ExpGiver.magnetToPlayer = true;
        AugmentManager.Instance.RemoveData(thisData);
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
