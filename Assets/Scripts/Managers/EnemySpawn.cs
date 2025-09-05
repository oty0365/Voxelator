using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

[System.Serializable]
public class EnemyUnlockRate
{
    public GameObject enemy;
    public int stage;
}

public class EnemySpawn : SceneSingletonMonoBehaviour<EnemySpawn>
{
    [Header("Spawn Settings")]
    public SpawnSettingsSO spawnSettings;
    
    [SerializeField]
    private float distance = 5f;
    private GameObject player;

    public EnemyBankSO normal;
    public EnemyBankSO elite;
    public EnemyBankSO boss;
    
    private List<GameObject> spawnableTable = new();
    
    private float currentSpawnCoolDown;
    
    void Start()
    {
        player = PlayerStatus.Instance.gameObject;
        currentSpawnCoolDown = spawnSettings.maxSpawnCoolDown;
        StartCoroutine(SpawnEnemy());
        StartCoroutine(IncreaseDifficulty());
    }
    
    /*public void TryUnlockEnemies(int currentScore)
    {
        List<EnemyUnlockRate> unlocked = new List<EnemyUnlockRate>();
        foreach (var data in spawnableTable)
        {
            if (data <= currentScore)
            {
                if (!enemy.Contains(data.enemy))
                {
                    enemy.Add(data.enemy);
                }
                unlocked.Add(data);
            }
        }
        
        foreach (var data in unlocked)
        {
            unlockRate.Remove(data);
        }
    }*/
    
    IEnumerator IncreaseDifficulty()
    {
        while (true)
        {
            yield return new WaitForSeconds(spawnSettings.difficultyIncreaseInterval);
            currentSpawnCoolDown = Mathf.Max(currentSpawnCoolDown * spawnSettings.difficultyIncreaseRate, spawnSettings.minSpawnCoolDown);
        }
    }
    
    IEnumerator SpawnEnemy()
    {
        while (true)
        {
            for (var i = 0; i < Random.Range(spawnSettings.minSpawnCount, spawnSettings.maxSpawnCount); i++)
            {
                int ran = Random.Range(0, 360);
                float x = Mathf.Cos(ran * Mathf.Deg2Rad) * distance;
                float y = Mathf.Sin(ran * Mathf.Deg2Rad) * distance;
                Vector3 pos = player.transform.position + new Vector3(x, y, 0);

                if (spawnableTable.Count > 0)
                {
                    ObjectPoolManager.Instance.Get(spawnableTable[Random.Range(0, spawnableTable.Count)], pos,
                        new Vector3(0, 0, 0));
                }
            }
            yield return new WaitForSeconds(currentSpawnCoolDown);
        }
    }
}
