using System;
using UnityEngine;

public class PannelCharacterConnecter : MonoBehaviour,IEvent,IPannelUI,IConnecter
{
    [SerializeField] private GameObject pannel;
    [SerializeField] private PlayerCharacter playerData;

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

    public void Subscribe()
    {
        EventManager.Instance.AddListener(EventKey.CharacterSelectPanelActive, new Action(OnActiveUI));
        EventManager.Instance.AddListener(EventKey.CharacterSelectPanelInactive, new Action(OnInactiveUI));
    }
    public void Unsubscribe()
    {
        if (EventManager.Instance != null)
        {
            EventManager.Instance.RemoveListener(EventKey.CharacterSelectPanelActive, new Action(OnActiveUI));
            EventManager.Instance.RemoveListener(EventKey.CharacterSelectPanelInactive, new Action(OnInactiveUI));
        }
    }

    public void OnConnect()
    {
        pannel.GetComponent<CharacterSelectPannel>().Initialize(playerData.GetComponent<IDataGetSetter<PlayerCharacterInfo>>(),playerData.GetComponent<ISaveAble>());
        _ipannel = pannel.GetComponent<IPannel>();
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
