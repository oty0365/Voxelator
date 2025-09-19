using UnityEngine;

[CreateAssetMenu(fileName = "SpawnSettingsSO", menuName = "Scriptable Objects/SpawnSettingsSO")]
public class SpawnSettingsSO : ScriptableObject
{
    public float maxSpawnCoolDown;
    public float minSpawnCoolDown;
    public int maxSpawnCount;
    public int minSpawnCount;
    public float difficultyIncreaseRate;
    public float difficultyIncreaseInterval;
    public float healthIncreaseInterval;
    public float attackIncreaseInterval;
    public float expIncreaseInterval;
}
