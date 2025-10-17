using System;
using System.Collections.Generic;
using UnityEngine;


public enum FightEventKey
{
    OnPlayerHit,        
}

public enum FieldEventKey
{
    OnClocked,        
    AddToSpawner,      
    SpawnElite,         
    StartSpawning,     
    StopSpawning,      
    ContinueSpawning,
    OnBossBattleStart,
    OnBossBattleEnd,    
    KillAllMonsters,
}

public enum UIEventKey
{
    ShowMapBanner,        
    ShowEventBanner,     
    OnTalkStart,       
    OnTalkEnd,          
    LevelUpPanelActive,  
    LevelUpPanelInactive, 
}

public class EventManager : SceneSingletonMonoBehaviour<EventManager>
{
    private readonly Dictionary<System.Type, object> _containers = new();
    
    protected override void Awake()
    {
        base.Awake();
        
        _containers[typeof(FightEventKey)] = new EventContainer<FightEventKey>();
        _containers[typeof(UIEventKey)] = new EventContainer<UIEventKey>();
        _containers[typeof(FieldEventKey)] = new EventContainer<FieldEventKey>();
    }
    
    private EventContainer<T> GetContainer<T>() where T : Enum
    {
        var type = typeof(T);
        if (_containers.TryGetValue(type, out var container))
        {
            return container as EventContainer<T>;
        }
        throw new KeyNotFoundException($"이벤트 컨테이너를 찾을 수 없음: {type}");
    }
    
    public void AddListener<T>(T key, MulticastDelegate listener) where T : Enum
    {
        GetContainer<T>().AddListener(key, listener);
    }
    
    public void RemoveListener<T>(T key, MulticastDelegate listener) where T : Enum
    {
        GetContainer<T>().RemoveListener(key, listener);
    }
    
    public void Invoke<T>(T key, params object[] args) where T : Enum
    {
        GetContainer<T>().Invoke(key, args);
    }
}
