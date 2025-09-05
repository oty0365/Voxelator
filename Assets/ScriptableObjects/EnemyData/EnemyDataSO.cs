using UnityEngine;

[CreateAssetMenu(fileName = "EnemyDataSO", menuName = "Scriptable Objects/EnemyDataSO")]
public class EnemyDataSO : ScriptableObject
{
    public RandomRanged health;
    public RandomRanged expDrop;
    public RandomRanged baseAttack;
    public RandomRanged baseDefense;
    public EntityType entityType;
    public float moveSpeed;
}
