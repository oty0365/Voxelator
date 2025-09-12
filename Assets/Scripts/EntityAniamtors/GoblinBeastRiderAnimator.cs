using UnityEngine;

public class GoblinBeastRiderAnimator : EntityAnimator
{
    private string _aniHash = "Behave";
    private string _aniTriggerHash1 = "DashPrepare";
    private string _aniTriggerHash2 = "Dash";
    private string _aniTriggerHash3 = "Roar";
    private string _aniTriggerHash4 = "Bite";

    public AnimationClip roarClip; 
    
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
            case EntityMoves.PrepareDash:
                ani.SetInteger(_aniHash, 2);
                ani.SetTrigger(_aniTriggerHash1);
                break;
            case EntityMoves.Dash:
                ani.SetInteger(_aniHash, 2);
                ani.SetTrigger(_aniTriggerHash2);
                break;
            case EntityMoves.Roar:
                ani.SetInteger(_aniHash, 2);
                ani.SetTrigger(_aniTriggerHash3);
                break;
            case EntityMoves.Bite:                
                ani.SetInteger(_aniHash, 2);
                ani.SetTrigger(_aniTriggerHash4);
                break;
        }
    }   

}
