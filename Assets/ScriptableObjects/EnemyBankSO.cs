using System;
using System.Collections.Generic;
using UnityEngine;

public enum EnemyCode
{
    Goblin,
    RockGolem,
    PlainChopper,
    EliteGoblinKnight,
    EliteGazer,
    BossGoblinBeastRider
}

[Serializable]
public class EnemySet
{
    public EnemyCode code;
    public GameObject enemyObj;    
}

[CreateAssetMenu(fileName = "EnemyBankSO", menuName = "Scriptable Objects/EnemyBankSO")]
public class EnemyBankSO : ScriptableObject
{
    public List<EnemySet> enemyBank;
}
