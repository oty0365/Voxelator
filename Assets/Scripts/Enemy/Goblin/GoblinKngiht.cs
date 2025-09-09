using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoblinKnight : AEnemy, IPoolingObject, IFootSteper
{
    [SerializeField] private FootStepEffect footStepEffect;
    [SerializeField] private GoblinKnightAnimator goblinKnightAnimator;
    public void StartStep()=>footStepEffect.StartStep();
    public void EndStep()=>footStepEffect.EndStep();
    
    private void Start()
    {
        Initialize();
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
        var moveTowards = new GoblinKnightRunST(this);
        var dashPrepare = new GoblinKnightDashPrepareST(this);
        var dash = new GoblinKnightDashST(this);
        fsm.RegisterState("Run", moveTowards);
        fsm.RegisterState("DashPrepare", dashPrepare);
        fsm.RegisterState("Dash", dash);
        fsm.ChangeState("Run");
    }

    public class GoblinKnightRunST : IState
    {
        private AEnemy _enemy;
        
        public GoblinKnightRunST(AEnemy enemy)
        {
            this._enemy = enemy;
        }
        
        public void Enter()
        {
            _enemy.gameObject.GetComponent<GoblinKnight>().goblinKnightAnimator.SetAnimation(EntityMoves.Walk);
            _enemy.rb2D.linearVelocity = (PlayerStatus.Instance.gameObject.transform.position-_enemy.gameObject.transform.position).normalized*_enemy.enemyData.moveSpeed.Value;
        }

        public void FixedExecute()
        {
            _enemy.rb2D.linearVelocity = (PlayerStatus.Instance.gameObject.transform.position-_enemy.gameObject.transform.position).normalized*_enemy.enemyData.moveSpeed.Value;
        }
        public void Execute()
        { 
            _enemy.FacePlayer();
            if (Vector2.Distance(_enemy.rb2D.position, PlayerStatus.Instance.gameObject.transform.position) < 5)
            {
                _enemy.fsm.ChangeState("DashPrepare");
            }
        }
        public void Exit()
        {
        
        }
    }
    public class GoblinKnightDashPrepareST:IState
    {
        private AEnemy _enemy;
        private float _startTime;
        private float[] _waitDuration = {3f,3.5f};
        private float _dashRange = 6f;
        private StaticLineIndicator _sli;

        public GoblinKnightDashPrepareST(AEnemy enemy)
        {
            _enemy = enemy;
        }

        public void Enter()
        {
            _startTime = Time.time;
            _enemy.rb2D.linearVelocity = Vector2.zero;
            _enemy.gameObject.GetComponent<GoblinKnight>().goblinKnightAnimator.SetAnimation(EntityMoves.PrepareDash);
            var o = ObjectPoolManager.Instance.Get(ObjectBankManager.Instance.Get(ObjectCode.StaticIndicatorLine),IndicatorCanvas.Instance.canvasPrefab.transform);
            var finalDestination = (PlayerStatus.Instance.gameObject.transform.position-_enemy.gameObject.transform.position).normalized*_dashRange;
            _sli = o.GetComponent<StaticLineIndicator>();
            _sli.SetTarget(_enemy.gameObject, _enemy.transform.position+finalDestination);
        }

        public void Execute()
        {
            var finalDestination = (PlayerStatus.Instance.gameObject.transform.position-_enemy.gameObject.transform.position).normalized*_dashRange;
            _sli.SetTarget(_enemy.gameObject,finalDestination);
            _enemy.FacePlayer();
            if (Time.time - _startTime >= _waitDuration[0])
            {
                _sli.OnExecute();
            }

            if (Time.time - _startTime >= _waitDuration[1])
            {
                _sli.OnExit();
                _enemy.fsm.ChangeState("dash");
            }
        }

        public void FixedExecute()
        {
            
        }

        public void Exit()
        {
            
        }
        
    }
    public class GoblinKnightDashST:IState
    {
        private AEnemy _enemy;
        private Vector2 _finalDestination;
        private float _dashSpeed = 12f;
        private float _dashRange = 6f;

        public GoblinKnightDashST(AEnemy enemy)
        {
            _enemy = enemy;
        }

        public void Enter()
        {
            _enemy.rb2D.linearVelocity  = Vector2.zero;
            _enemy.gameObject.GetComponent<GoblinKnight>().goblinKnightAnimator.SetAnimation(EntityMoves.Dash);
            var o = ObjectBankManager.Instance.Get(ObjectCode.DynamicIndicatorLine);
            var dir = (PlayerStatus.Instance.gameObject.transform.position - _enemy.gameObject.transform.position).normalized;
            _finalDestination = dir*_dashRange;
            _enemy.rb2D.linearVelocity  = dir*_dashSpeed;
        }

        public void Execute()
        {
            if (Vector2.Distance(_enemy.rb2D.position, _finalDestination) <= 0.02f)
            {
                _enemy.rb2D.linearVelocity  = Vector2.zero;
                _enemy.fsm.ChangeState("Run");
            }
        }

        public void FixedExecute()
        {

        }

        public void Exit()
        {
            
        }
        
    }
    
}