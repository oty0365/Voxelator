using UnityEngine;

[CreateAssetMenu(fileName = "EnemyData", menuName = "Scriptable Objects/EnemyData")]
public class EnemyDataSO : ScriptableObject
{
    public RandomRanged health;
    public RandomRanged expDrop;
    public RandomRanged baseAttack;
    public RandomRanged baseDefense;
    public EntityType entityType;
    public float moveSpeed;
}
