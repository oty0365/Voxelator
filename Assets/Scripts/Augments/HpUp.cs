using UnityEngine;

public class HpUp : AAugment,IPoolingObject
{
    [SerializeField] private AugmentedDatasSO augmentedDatasSO;
    public override void Execute()
    {
        PlayerStatus.Instance.playerHp.SetMaxBuff(BuffType.Add,Extracter.Instance.ParseFloat(augmentedDatasSO.datas[0]));
        //PlayerStatus.Instance.playerHp.SetBuff(BuffType.Add,Extracter.Instance.ParseFloat(augmentedDatasSO.datas[0]));
    }

    public void OnBirth()
    {
        Execute();
    }

    public void OnDeathInit()
    {
    }
}
