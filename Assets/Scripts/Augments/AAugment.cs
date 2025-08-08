using UnityEngine;

public abstract class AAugment : MonoBehaviour
{
    public AugmentData keyData;

    public abstract void Execute();
    public void UpLoad() 
    {
        AugmentManager.Instance.ConsumedAugment(keyData);
    }
}
