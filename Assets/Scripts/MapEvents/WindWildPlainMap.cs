using System;
using UnityEngine;

public class WindWildPlainMap : AMap,IEvent
{

    [SerializeField] private DialogsSO[] mapDia;
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
                DialogManager.Instance.StartConversation(mapDia[0]);
                break;
            case 6:
                AugmentManager.Instance.AugmentSelection(AugmentState.Weapon);
                break;
            case 7:
                EventManager.Instance.Invoke(EventKey.StartSpawning);
                break;
            case 70:
                EventManager.Instance.Invoke(EventKey.AddToSpawner,EnemyCode.Goblin);
                break;
            case 110:
                EventManager.Instance.Invoke(EventKey.ShowEventBanner,"EliteSpawn");
                EventManager.Instance.Invoke(EventKey.SpawnElite,EnemyCode.EliteGoblinKnight);
                break;
            case 200:
                EventManager.Instance.Invoke(EventKey.AddToSpawner,EnemyCode.RockGolem);
                break;
            case 220:
                EventManager.Instance.Invoke(EventKey.ShowEventBanner,"EliteSpawn");
                EventManager.Instance.Invoke(EventKey.SpawnElite,EnemyCode.EliteGoblinKnight);
                EventManager.Instance.Invoke(EventKey.SpawnElite,EnemyCode.EliteGoblinKnight);
                break;
            case 280:
                EventManager.Instance.Invoke(EventKey.StopSpawning);
                EventManager.Instance.Invoke(EventKey.KillAllMonsters);
                TimeManager.Instance.StopGame();
                DialogManager.Instance.StartConversation(mapDia[1]);
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
