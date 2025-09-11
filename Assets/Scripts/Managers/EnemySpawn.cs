using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class EnemySpawn : SceneSingletonMonoBehaviour<EnemySpawn>,IEvent
{
    [Header("Spawn Settings")]
    public SpawnSettingsSO spawnSettings;
    
    [SerializeField]
    private float distance = 5f;
    private GameObject player;

    public EnemyBankSO normal;
    public EnemyBankSO elite;
    public EnemyBankSO boss;

    private Dictionary<EnemyCode, GameObject> _normalDict = new();
    private Dictionary<EnemyCode, GameObject> _eliteDict = new();
    private Dictionary<EnemyCode, GameObject> _bossDict = new();
    
    private List<GameObject> _spawnableTable = new();
    private List<GameObject> _mapMonsters = new();
    
    private float _currentSpawnCoolDown;
    private Coroutine _currentSpawnFlow;
    private Coroutine _currentIncreaseFlow;
    
    void Start()
    {
        player = PlayerStatus.Instance.gameObject;
        _currentSpawnCoolDown = spawnSettings.maxSpawnCoolDown;
        Initialize();
    }

    public void Initialize()
    {
        foreach (var n in normal.enemyBank)
        {
            if (!_normalDict.ContainsKey(n.code))
            {
                _normalDict.Add(n.code, n.enemyObj);
            }
        }

        foreach (var e in elite.enemyBank)
        {
            if (!_eliteDict.ContainsKey(e.code))
            {
                _eliteDict.Add(e.code, e.enemyObj);
            }
        }

        foreach (var b in boss.enemyBank)
        {
            if (!_bossDict.ContainsKey(b.code))
            {
                _bossDict.Add(b.code, b.enemyObj);
            }
        }
    }

    public void SpawnElite(EnemyCode code)
    {
        int ran = Random.Range(0, 360);
        float x = Mathf.Cos(ran * Mathf.Deg2Rad) * distance;
        float y = Mathf.Sin(ran * Mathf.Deg2Rad) * distance;
        Vector3 pos = player.transform.position + new Vector3(x, y, 0);
        ObjectPoolManager.Instance.Get(_eliteDict[code], pos, new Vector3(0, 0, 0));
    }
    
    public void TryUnlockEnemy(EnemyCode code)
    {            
        if (_normalDict.ContainsKey(code)&&!_spawnableTable.Contains(_normalDict[code]))
        {
            _spawnableTable.Add(_normalDict[code]);
        }
    }

    public void StartSpawn()
    {
        _currentSpawnCoolDown = spawnSettings.maxSpawnCoolDown;
        ContinueSpawn();
    }

    public void UpLoadToList(GameObject obj)
    {
        _mapMonsters.Add(obj);
    }

    public void RemoveInList(GameObject obj)
    {
        _mapMonsters.Remove(obj);
    }

    public void RemoveAllMonstersInMap()
    {
        for (var m=0; m< _mapMonsters.Count;m++)
        {
            _mapMonsters[m].GetComponent<AEnemy>().Death();
        }
        _mapMonsters.Clear();
    }

    public void ContinueSpawn()
    {
        if (_currentSpawnFlow != null)
        {
            StopCoroutine(_currentSpawnFlow);
        }
        if (_currentIncreaseFlow != null)
        {
            StopCoroutine(_currentIncreaseFlow);
        }
        _currentIncreaseFlow = StartCoroutine(IncreaseDifficultyFlow());
        _currentSpawnFlow = StartCoroutine(SpawnEnemyFlow());
    }

    public void StopSpawn()
    {
        if (_currentSpawnFlow != null)
        {
            StopCoroutine(_currentSpawnFlow);
            _currentSpawnFlow = null;
        }
        if (_currentIncreaseFlow != null)
        {
            StopCoroutine(_currentIncreaseFlow);
            _currentIncreaseFlow = null;
        }
    }
    
    IEnumerator IncreaseDifficultyFlow()
    {
        while (true)
        {
            yield return new WaitForSeconds(spawnSettings.difficultyIncreaseInterval);
            _currentSpawnCoolDown *= spawnSettings.difficultyIncreaseRate;
            _currentSpawnCoolDown = Mathf.Clamp(_currentSpawnCoolDown, spawnSettings.minSpawnCoolDown, spawnSettings.maxSpawnCoolDown); 
        }
    }
    
    IEnumerator SpawnEnemyFlow()
    {
        while (true)
        {
            for (var i = 0; i < Random.Range(spawnSettings.minSpawnCount, spawnSettings.maxSpawnCount); i++)
            {
                int ran = Random.Range(0, 360);
                float x = Mathf.Cos(ran * Mathf.Deg2Rad) * distance;
                float y = Mathf.Sin(ran * Mathf.Deg2Rad) * distance;
                Vector3 pos = player.transform.position + new Vector3(x, y, 0);

                if (_spawnableTable.Count > 0)
                {
                    ObjectPoolManager.Instance.Get(_spawnableTable[Random.Range(0, _spawnableTable.Count)], pos,
                        new Vector3(0, 0, 0));
                }
            }
            yield return new WaitForSeconds(_currentSpawnCoolDown);
        }
    }

    public void Subscribe()
    {
        EventManager.Instance.AddListener(EventKey.KillAllMonsters, new Action(RemoveAllMonstersInMap));
        EventManager.Instance.AddListener(EventKey.SpawnElite, new Action<EnemyCode>(SpawnElite));
        EventManager.Instance.AddListener(EventKey.AddToSpawner, new Action<EnemyCode>(TryUnlockEnemy));
        EventManager.Instance.AddListener(EventKey.StartSpawning,new Action(StartSpawn));
        EventManager.Instance.AddListener(EventKey.ContinueSpawning,new Action(ContinueSpawn));
        EventManager.Instance.AddListener(EventKey.StopSpawning,new Action(StopSpawn));
    }

    public void Unsubscribe()
    {
        EventManager.Instance.RemoveListener(EventKey.KillAllMonsters, new Action(RemoveAllMonstersInMap));
        EventManager.Instance.RemoveListener(EventKey.SpawnElite, new Action<EnemyCode>(SpawnElite));
        EventManager.Instance.RemoveListener(EventKey.AddToSpawner, new Action<EnemyCode>(TryUnlockEnemy));
        EventManager.Instance.RemoveListener(EventKey.StartSpawning,new Action(StartSpawn));
        EventManager.Instance.RemoveListener(EventKey.ContinueSpawning,new Action(ContinueSpawn));
        EventManager.Instance.RemoveListener(EventKey.StopSpawning,new Action(StopSpawn));
    }

    public void OnEnable()
    {
        Subscribe();
    }

    public void OnDisable()
    {
        Unsubscribe();
    }
}
