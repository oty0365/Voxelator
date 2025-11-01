using System;
using UnityEngine;

public class PannelStatusConnecter : MonoBehaviour,IPannelUI,IConnecter
{
    [SerializeField] private GameObject button;
    [SerializeField] private GameObject pannel;
    [SerializeField] private GameObject playerData;
    
    private PlayerStatUpgradeInfo _upgradeInfo = new();

    private IPannel _ipannel;

    private void Start()
    {
        OnConnect();
    }

    public void OnActiveUI()
    {
        pannel.SetActive(true);
        _ipannel.OnActive();
        
    }
    public void OnInactiveUI()
    {
        pannel.SetActive(false);
        _ipannel.OnInActive();
    }
    public void OnConnect()
    {
        playerData.GetComponent<IDataGetSetter<PlayerStatUpgradeInfo>>().Get(_upgradeInfo);
        RuntimeUpgradeStatManager.Instance.SetAtk(_upgradeInfo.atk);
        RuntimeUpgradeStatManager.Instance.SetDef(_upgradeInfo.def);
        RuntimeUpgradeStatManager.Instance.SetHp(_upgradeInfo.hp);
        button.GetComponent<IButton>().onClick+=OnActiveUI;
        pannel.GetComponent<LevelUpPannel>().Initialize(playerData.GetComponent<IDataGetSetter<PlayerStatUpgradeInfo>>(),playerData.GetComponent<ISaveAble>());
        _ipannel = pannel.GetComponent<IPannel>();
    }
}
