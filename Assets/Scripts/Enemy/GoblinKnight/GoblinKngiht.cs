using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoblinKnight : AEnemy, IPoolingObject, IFootSteper
{
    [SerializeField] private FootStepEffect footStepEffect;
    [SerializeField] private GoblinKnightAnimator goblinKnightAnimator;
    public EnemySkillCooldown enemySkillCooldown = new();
    public void StartStep()=>footStepEffect.StartStep();
    public void EndStep()=>footStepEffect.EndStep();

    [NonSerialized] public Vector2 finalDestination;
    
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
        var moveTowards = new GoblinKnightRunST(this);
        var dashPrepare = new GoblinKnightDashPrepareST(this);
        var dash = new GoblinKnightDashST(this);
        fsm.RegisterState("Run", moveTowards);
        fsm.RegisterState("DashPrepare", dashPrepare);
        fsm.RegisterState("Dash", dash);
        enemySkillCooldown.AddPattern("Dash",1.5f);
        fsm.ChangeState("Run");
    }

    public class GoblinKnightRunST : IState
    {
        private AEnemy _enemy;
        private GoblinKnight _goblinKnight;
        
        public GoblinKnightRunST(AEnemy enemy)
        {
            this._enemy = enemy;
        }
        
        public void Enter()
        {
            _goblinKnight=_enemy.gameObject.GetComponent<GoblinKnight>(); 
            _goblinKnight.goblinKnightAnimator.SetAnimation(EntityMoves.Walk);
            _enemy.rb2D.linearVelocity = (PlayerStatus.Instance.gameObject.transform.position-_enemy.gameObject.transform.position).normalized*_enemy.enemyData.moveSpeed.Value;
        }

        public void FixedExecute()
        {
            _enemy.rb2D.linearVelocity = (PlayerStatus.Instance.gameObject.transform.position-_enemy.gameObject.transform.position).normalized*_enemy.enemyData.moveSpeed.Value;
        }
        public void Execute()
        {
            _goblinKnight.enemySkillCooldown.DecreaseCooldown("Dash",Time.deltaTime);
            _enemy.FacePlayer();
            if (Vector2.Distance(_enemy.rb2D.position, PlayerStatus.Instance.gameObject.transform.position) < 5&&_goblinKnight.enemySkillCooldown.CheckToUseSkill("Dash"))
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
        private float[] _waitDuration = {1f,1.5f};
        private float _dashRange = 6f;
        private StaticLineIndicator _sli;
        private Vector3 _finalDestination;

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
            _sli.SetParent(_enemy.gameObject);
            _sli.SetTarget(_enemy.gameObject, _enemy.transform.position+finalDestination, new Vector3(0.5f, 0.5f, 0f));
        }

        public void Execute()
        {
            if (Time.time - _startTime >= _waitDuration[0])
            {

                _sli.OnExecute();
            }
            else
            {
                _finalDestination = (PlayerStatus.Instance.gameObject.transform.position-_enemy.gameObject.transform.position).normalized*_dashRange;
                _sli.SetTarget(_enemy.gameObject,_enemy.transform.position+_finalDestination,new Vector3(0.5f, 0.5f, 0f));
                _enemy.FacePlayer();
            }

            if (Time.time - _startTime >= _waitDuration[1])
            {
                _sli.OnExit();
                _enemy.GetComponent<GoblinKnight>().finalDestination =  _finalDestination/_dashRange;
                _enemy.fsm.ChangeState("Dash");
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
        private AfterImageGenerator _generator;

        public GoblinKnightDashST(AEnemy enemy)
        {
            _enemy = enemy;
        }

        public void Enter()
        {
            _enemy.rb2D.linearVelocity  = Vector2.zero;
            _enemy.gameObject.GetComponent<GoblinKnight>().goblinKnightAnimator.SetAnimation(EntityMoves.Dash);
            var o = ObjectPoolManager.Instance.Get(ObjectBankManager.Instance.Get(ObjectCode.AfterImageGenerator),Vector2.zero,Vector3.zero);
            _generator = o.GetComponent<AfterImageGenerator>();
            _generator.StartSpawn(_enemy.gameObject,0.05f);
            var dir = _enemy.GetComponent<GoblinKnight>().finalDestination;
            _finalDestination =dir*_dashRange+_enemy.rb2D.position;
            _enemy.rb2D.linearVelocity  = dir*_dashSpeed;
            _enemy.enemyData.baseAttack.SetBuff(BuffType.Mul,2);
        }

        public void Execute()
        {
            if (Vector2.Distance(_enemy.rb2D.position, _finalDestination) <= 0.1f)
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
            _enemy.enemyData.baseAttack.SetBuff(BuffType.Mul,-2);
            ObjectPoolManager.Instance.Return(_generator.gameObject);
        }
        
    }
    
}