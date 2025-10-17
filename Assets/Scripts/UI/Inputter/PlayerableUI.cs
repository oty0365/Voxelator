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
        EventManager.Instance.AddListener(UIEventKey.OnTalkStart,new Action(Disappear));
        EventManager.Instance.AddListener(UIEventKey.OnTalkEnd, new Action(Appear));
    }

    public void Unsubscribe()
    {
        EventManager.Instance.RemoveListener(UIEventKey.OnTalkStart,new Action(Disappear));
        EventManager.Instance.RemoveListener(UIEventKey.OnTalkEnd, new Action(Appear));
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
