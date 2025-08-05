using UnityEngine;

[CreateAssetMenu(fileName = "PlayerMovementSO", menuName = "Scriptable Objects/PlayerMovementSO")]
public class PlayerMovementSO : ScriptableObject
{
    [Header("�뽬")]
    public float dashSpeed;
    public float dashTime;
    public float dashCooldown;
}
