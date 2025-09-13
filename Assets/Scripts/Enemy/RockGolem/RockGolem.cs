using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RockGolem : AEnemy, IPoolingObject, IFootSteper
{
    [SerializeField] private FootStepEffect footStepEffect;
    public void StartStep()=>footStepEffect.StartStep();
    public void EndStep()=>footStepEffect.EndStep();
    
    private void Start()
    {
        //Initialize();
    }
    public void OnBirth()
    {
        Initialize();

    }

    public void OnDeathInit()
    {
        EndStep();
    }

    public override void Initialize()
    {
        base.Initialize();
        StartStep();
        var moveTowards = new EnemyMoveTowardsNonFlipST(this);
        fsm.RegisterState("Run", moveTowards);
        fsm.ChangeState("Run");
    }
}