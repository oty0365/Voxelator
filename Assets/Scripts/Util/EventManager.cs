using System;
using System.Collections.Generic;
using UnityEngine;

public enum EventKey
{
    //전투
    OnPlayerHit,
    OnBossBattleStart,
    OnBossBattleEnd,
    KillAllMonsters,
    
    //필드
    OnClocked,
    AddToSpawner,
    SpawnElite,
    StartSpawning,
    StopSpawning,
    ContinueSpawning,
    SetTimeScale,
    
    //카메라
    ConnectWithCameraPlayerStaus,
    StopShake,
    
    //UI
    ShowMapBanner,
    ShowEventBanner,
    OnTalkStart,
    OnTalkEnd,
    LevelUpPanelActive,
    LevelUpPanelInactive,
    CharacterSelectPanelActive,
    CharacterSelectPanelInactive,
}

public class EventManager : SceneSingletonMonoBehaviour<EventManager>
{
    private readonly Dictionary<EventKey, MulticastDelegate> _events = new();
    
    public void AddListener(EventKey actionKey, MulticastDelegate listener)
    {
        if (_events.TryGetValue(actionKey, out var existing))
        {
            if (existing.GetType() != listener.GetType())
            {
                throw new InvalidOperationException($"이벤트 [{actionKey}]는 {existing.GetType()} 형식만 받을 수 있음. 현재: {listener.GetType()}");
            }
            _events[actionKey] = (MulticastDelegate)MulticastDelegate.Combine(existing, listener);
        }
        else
        {
            _events[actionKey] = listener;
        }
    }
    
    public void RemoveListener(EventKey actionKey, MulticastDelegate listener)
    {
        if (_events.TryGetValue(actionKey, out var existing))
        {
            if (existing.GetType() != listener.GetType())
            {
                throw new InvalidOperationException($"이벤트 [{actionKey}] 구독 해제 시 형식 불일치. 기대: {existing.GetType()} / 현재: {listener.GetType()}");
            }

            var updated = existing;
            var invocationList = existing.GetInvocationList();
    
            foreach (var del in invocationList)
            {
                if (del.Method == listener.Method && 
                    ReferenceEquals(del.Target, listener.Target))
                {
                    updated = (MulticastDelegate)MulticastDelegate.Remove(updated, del);
                    break; 
                }
            }
    
            if (updated == null)
                _events.Remove(actionKey);
            else
                _events[actionKey] = updated;
        }
    }
    
    public void Invoke(EventKey actionKey, params object[] args)
    {
        if (_events.TryGetValue(actionKey, out var del))
        {
            del.DynamicInvoke(args);
        }
    }
}