using System;
using UnityEngine;

public class PlayerableUI : MonoBehaviour,IEvent
{
    public GameObject joystick;
    
    public void Appear()
    {
        joystick.SetActive(true);
    }

    public void Disappear()
    {
        joystick.SetActive(false);
    }

    public void Subscribe()
    {
        EventManager.Instance.AddListener(EventKey.OnTalkStart,new Action(Disappear));
        EventManager.Instance.AddListener(EventKey.OnTalkEnd, new Action(Appear));
    }

    public void Unsubscribe()
    {
        EventManager.Instance.RemoveListener(EventKey.OnTalkStart,new Action(Disappear));
        EventManager.Instance.RemoveListener(EventKey.OnTalkEnd, new Action(Appear));
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
