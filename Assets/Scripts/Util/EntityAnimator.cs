using UnityEngine;

public enum EntityMoves
{
    Idle,
    Walk,
    Dash,
    PrepareDash,
    
}

public abstract class EntityAnimator : MonoBehaviour
{
    [SerializeField] protected Animator ani;

    public virtual void SetAnimation(EntityMoves moves)
    {
        
    }
    public virtual void ChangeAnimation(string code)
    {
        
    }
}
