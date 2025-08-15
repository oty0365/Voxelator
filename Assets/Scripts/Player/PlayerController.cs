using System;
using UnityEngine;

public enum PlayerMoves
{
    Idle,
    Walk,
    Dash
}

public class PlayerController : SceneSingletonMonoBehaviour<PlayerController>,IEvent
{
    [SerializeField] private Rigidbody2D rb2D;
    [SerializeField] private PlayerAnimator playerAnimator;

    private event Action<PlayerMoves> playerMovesAnimator;
    private PlayerMoves _playerMoves;

    public PlayerMovementData playerMovementData;
    public PlayerDash playerDash;
    public Vector2 currentDir;
    public PlayerMoves playerMoves 
    {
        get => _playerMoves;
        set
        {
            if (value != _playerMoves) 
            {
                _playerMoves = value;
                playerMovesAnimator?.Invoke(_playerMoves);
            }
        }
    }

    private void FixedUpdate()
    {
        rb2D.linearVelocity = currentDir*PlayerStatus.Instance.playerMoveSpeed.Value*playerDash.currentDashSpeed;
    }

    public void Subscribe()
    {
        playerMovesAnimator += playerAnimator.SetAnimation;
    }
    public void Unsubscribe()
    {
        playerMovesAnimator -= playerAnimator.SetAnimation;
    }
    private void OnEnable()
    {
        Subscribe();
    }
    private void OnDisable()
    {
        Unsubscribe();
    }
}
