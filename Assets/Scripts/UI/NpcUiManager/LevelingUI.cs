using System;
using UnityEngine;

public class LevelingUI : MonoBehaviour,IEvent,IPannelUI
{
    [SerializeField] private GameObject pannel;
    [SerializeField] private PlayerCharacter playerData;

    private IPannel _ipannel;

    private void Start()
    {
        pannel.GetComponent<LevelUpPannel>().Initialize(playerData.GetComponent<IDataGetSetter<PlayerStatUpgradeInfo>>(),playerData.GetComponent<ISaveable>());
        _ipannel = pannel.GetComponent<IPannel>();
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

    public void Subscribe()
    {
        EventManager.Instance.AddListener(EventKey.LevelUpPanelActive, new Action(OnActiveUI));
        EventManager.Instance.AddListener(EventKey.LevelUpPanelInactive, new Action(OnInactiveUI));
    }
    public void Unsubscribe()
    {
        if (EventManager.Instance != null)
        {
            EventManager.Instance.RemoveListener(EventKey.LevelUpPanelActive, new Action(OnActiveUI));
            EventManager.Instance.RemoveListener(EventKey.LevelUpPanelInactive, new Action(OnInactiveUI));
        }
    }
    private void OnEnable()
    {
        Subscribe();
    }
    private void OnDisable()
    {
        Unsubscribe();
    }
}
