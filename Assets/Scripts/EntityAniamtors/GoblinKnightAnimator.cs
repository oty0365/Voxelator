using UnityEngine;

public class GoblinKnightAnimator : EntityAnimator
{
    private string _aniHash = "Behave";
    private string _aniTriggerHash1 = "DashPrepare";
    private string _aniTriggerHash2 = "Dash";
    
    public override void SetAnimation(EntityMoves playerMoves)
    {
        switch (playerMoves) 
        {
            case EntityMoves.Idle:
                ani.SetInteger(_aniHash, 0);
                break;
            case EntityMoves.PrepareDash:
                ani.SetInteger(_aniHash, 1);
                ani.SetTrigger(_aniTriggerHash1);
                break;
            case EntityMoves.Dash:
                ani.SetInteger(_aniHash, 1);
                ani.SetTrigger(_aniTriggerHash2);
                break;
        }
    }   

}
