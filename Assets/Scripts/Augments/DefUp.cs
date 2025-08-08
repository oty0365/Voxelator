using UnityEngine;

public class DefUp : AAugment,IPoolingObject
{
    [SerializeField] AugmentedDatasSO augmentedDatasSO;
    public override void Execute()
    {
        PlayerStatus.Instance.SetDef(PlayerStatus.Instance.playerDef.Value + Extracter.Instance.ParseFloat(augmentedDatasSO.datas[0]));
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
