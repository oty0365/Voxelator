using UnityEngine;

[CreateAssetMenu(fileName = "PlayerBasicStatusData", menuName = "Scriptable Objects/PlayerBasicStatusData")]
public class PlayerBasicStatusData : ScriptableObject
{
    public float playerMaxHp;
    public float playerMoveSpeed;
    public float playerAtk;
    public float playerDef;
    public float playerMaxExp;
    public float playerAttackSpeed;
    public int playerBulletCount;
}
