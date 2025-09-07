using System;
using UnityEngine;

public class WindWildPlainMap : AMap,IEvent
{

    [SerializeField] private DialogsSO currentTutorialDia;
    public override void Execute(int time)
    {
        switch (time)
        {
            case 1:
                EventManager.Instance.Invoke(EventKey.ShowMapBanner);
                EventManager.Instance.Invoke(EventKey.AddToSpawner,EnemyCode.PlainChopper);
                break;
            case 5:
                TimeManager.Instance.StopGame();
                DialogManager.Instance.StartConversation(currentTutorialDia);
                //EventManager.Instance.Invoke(EventKey.StartSpawning);
                break;
            case 110:
                EventManager.Instance.Invoke(EventKey.AddToSpawner,EnemyCode.Goblin);
                Debug.Log(1);
                //EventManager.Instance.Invoke(EventKey.SpawnElite);
                break;
            case 200:
                EventManager.Instance.Invoke(EventKey.AddToSpawner,EnemyCode.RockGolem);
                break;
            case 220:
                //EventManager.Instance.Invoke(EventKey.SpawnElite);
                break;
            default:
                Debug.Log("타임라인 불일치");
                break;
        }
    
    }
    public void Subscribe()
    {
        EventManager.Instance.AddListener(EventKey.OnClocked,new Action<int>(CheckTime));
    }

    public void Unsubscribe()
    {
        EventManager.Instance.RemoveListener(EventKey.OnClocked,new Action<int>(CheckTime));
    }

    public void OnEnable()
    {
        Initialize();
        Subscribe();
    }

    public void OnDisable()
    {
        Unsubscribe();
    }
}
