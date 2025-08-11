using UnityEngine;

public class PlayerAnimator : MonoBehaviour
{
    private Animator ani;
    private string _aniHash = "Behave";
    private string _aniTriggerHash = "Dash";
    void Start()
    {
        ani=gameObject.GetComponent<Animator>();
    }
    public void SetAnimation(PlayerMoves playerMoves)
    {
        switch (playerMoves) 
        {
            case PlayerMoves.Idle:
                ani.SetInteger(_aniHash, 0);
                break;
            case PlayerMoves.Walk:
                ani.SetInteger(_aniHash, 1);
                break;
            case PlayerMoves.Dash:
                ani.SetInteger(_aniHash, 2);
                ani.SetTrigger(_aniTriggerHash);
                break;
        }
    }    
}
