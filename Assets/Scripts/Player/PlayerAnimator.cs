using UnityEngine;

public class PlayerAnimator : EntityAnimator
{
    private string _aniHash = "Behave";
    private string _aniTriggerHash = "Dash";
    
    public override void SetAnimation(EntityMoves playerMoves)
    {
        switch (playerMoves) 
        {
            case EntityMoves.Idle:
                ani.SetInteger(_aniHash, 0);
                break;
            case EntityMoves.Walk:
                ani.SetInteger(_aniHash, 1);
                break;
            case EntityMoves.Dash:
                ani.SetInteger(_aniHash, 2);
                ani.SetTrigger(_aniTriggerHash);
                break;
        }
    }    
}
