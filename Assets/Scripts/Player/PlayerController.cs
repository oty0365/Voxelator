using UnityEngine;

public enum PlayerMoves
{
    None,
    Walk,
    Dash
}

public class PlayerController : HalfSingleMono<PlayerController>
{
    [SerializeField] private Rigidbody2D rb2D;
    public PlayerMovementData playerMovementData;
    public PlayerDash playerDash;
    public Vector2 currentDir;
    public PlayerMoves playerMoves;
    private void FixedUpdate()
    {
        rb2D.linearVelocity = currentDir*PlayerStatus.Instance.playerMoveSpeed.Value*playerDash.currentDashSpeed;
    }
}
