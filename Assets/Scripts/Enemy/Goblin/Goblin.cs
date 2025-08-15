using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Goblin : Enemy,IPoolingObject
{
    private void Start()
    {
        Initialize();
        var moveTowards = new GoblinMoveTowards(this);
        fsm.RegisterState("Run", moveTowards);
        fsm.ChangeState("Run");
    }
    public void OnBirth()
    {
        Initialize();
        var moveTowards = new GoblinMoveTowards(this);
        fsm.RegisterState("Run", moveTowards);
        fsm.ChangeState("Run");
    }

    public void OnDeathInit()
    {
        
    }
}
public class GoblinMoveTowards : IState
{
    private Enemy _enemy;
        
    public GoblinMoveTowards(Enemy enemy)
    {
        this._enemy = enemy;
    }
        
    public void Enter()
    {
        _enemy.rb2D.linearVelocity = (PlayerStatus.Instance.gameObject.transform.position-_enemy.gameObject.transform.position).normalized*_enemy.enemyData.moveSpeed.Value;
    }

    public void FixedExecute()
    {
        _enemy.rb2D.linearVelocity = (PlayerStatus.Instance.gameObject.transform.position-_enemy.gameObject.transform.position).normalized*_enemy.enemyData.moveSpeed.Value;
    }
    public void Execute()
    { 
        _enemy.FacePlayer();
    }
    public void Exit()
    {
        
    }
}
