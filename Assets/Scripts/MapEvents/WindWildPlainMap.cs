using System;
using System.Collections;
using UnityEngine;

public class WindWildPlainMap : AMap,IEvent
{

    [SerializeField] private DialogsSO[] mapDia;
    [SerializeField] private Transform playerBossBattlePos;
    [SerializeField] private Transform bossSpawnPos;
    private AEnemy _currentBoss;
    public override void Execute(int time)
    {
        switch (time)
        {
            case 1:
                SoundManager.Instance.PlayBGM("Pixel Odyssey");
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
                TimeManager.Instance.StopGame();
                EventManager.Instance.Invoke(EventKey.StopSpawning);
                EventManager.Instance.Invoke(EventKey.KillAllMonsters);
                PlayerStatus.Instance.gameObject.transform.position = playerBossBattlePos.position;
                DialogManager.Instance.StartConversation(mapDia[1]);
                break;
            case 281:
                var boss=ObjectPoolManager.Instance.Get(EnemySpawn.Instance.GetBoss(EnemyCode.BossGoblinBeastRider),bossSpawnPos.position, Vector3.zero);
                _currentBoss=boss.GetComponent<AEnemy>();
                MapManager.Instance.SetBossBattle();
                EventManager.Instance.Invoke(EventKey.ShowMapBanner);
                SoundManager.Instance.PlayBGM("Beast Rider Clash");
                CameraManager.Instance.SetTarget(bossSpawnPos);
                StartCoroutine(EndCutSceneFlow());
                //CameraManager.Instance.SetTarget(bossSpawnPos);
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
        if (EventManager.Instance != null)
        {
            EventManager.Instance.RemoveListener(EventKey.OnClocked,new Action<int>(CheckTime));
        }
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

    private IEnumerator EndCutSceneFlow()
    {
        yield return new WaitForSeconds(3.5f);
        CameraManager.Instance.SetTarget(PlayerStatus.Instance.gameObject.transform);
        EventManager.Instance.Invoke(EventKey.OnBossBattleStart,"GoblinBeastRiderName",_currentBoss);
    }
}
