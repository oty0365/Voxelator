using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoblinBeastRider : AEnemy, IPoolingObject, IFootSteper
{
    [SerializeField] private FootStepEffect footStepEffect;
    [SerializeField] private GoblinBeastRiderAnimator goblinBeastRiderAnimator;
    public EnemySkillCooldown enemySkillCooldown = new();
    public void StartStep()=>footStepEffect.StartStep();
    public void EndStep()=>footStepEffect.EndStep();

    [SerializeField] private float startDelay;
    public GameObject spawnEnemy;
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

    public void RoarShake()
    {
        CameraManager.Instance.ShakeCamera(0.12f,100);
    }

    public void StopRoarShake()
    {
        CameraManager.Instance.StopShake();
    }
    
    public override void Initialize()
    {
        base.Initialize();
        StartStep();
        var idle = new GoblinBeastRiderIdleST(this);
        var moveTowards = new GoblinBeastRiderRunST(this);
        var dashPrepare = new GoblinBeastRiderDashPrepareST(this);
        var dash = new GoblinBeastRiderDashST(this);
        var roar = new GoblinBeastRiderRoarST(this);
        fsm.RegisterState("Idle",idle);
        fsm.RegisterState("Run", moveTowards);
        fsm.RegisterState("DashPrepare", dashPrepare);
        fsm.RegisterState("Dash", dash);
        fsm.RegisterState("Roar", roar);
        enemySkillCooldown.AddPattern("Dash",2.5f);
        enemySkillCooldown.AddPattern("Roar",12f);
        StartCoroutine(StartBossBattleFlow());

    }

    IEnumerator StartBossBattleFlow()
    {
        fsm.ChangeState("Idle");
        yield return new WaitForSeconds(startDelay);
        fsm.ChangeState("Roar");
        enemySkillCooldown.ReSetSkillCool("Roar");
        
    }
    public class GoblinBeastRiderRoarST : IState
    {
        private AEnemy _enemy;
        private GoblinBeastRider _goblinBeastRider;
        private float _waitLength;
        private float _currentTime;
        private float _spawnDistance = 1f;
        
        public GoblinBeastRiderRoarST(AEnemy enemy)
        {
            this._enemy = enemy;
        }
        
        public void Enter()
        {
            _currentTime = Time.time;
            _enemy.rb2D.linearVelocity = Vector2.zero;
            _goblinBeastRider = _enemy.gameObject.GetComponent<GoblinBeastRider>();
            _goblinBeastRider.goblinBeastRiderAnimator.SetAnimation(EntityMoves.Roar);
            _waitLength = _goblinBeastRider.goblinBeastRiderAnimator.roarClip.length;
        }

        public void Execute()
        {
            if (Time.time - _currentTime >= _waitLength)
            {
                int[] dirX = { 1, -1, 1, -1 };
                int[] dirY = { 1, -1, -1, 1 };
                for (int i = 0; i < 4; i++)
                {
                    ObjectPoolManager.Instance.Get(_goblinBeastRider.spawnEnemy, new Vector2(_enemy.transform.position.x + dirX[i]*_spawnDistance, _enemy.transform.position.y + dirY[i]*_spawnDistance), Vector3.zero);
                }
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

    public class GoblinBeastRiderIdleST : IState
    {
        private AEnemy _enemy;
        private GoblinBeastRider _goblinBeastRider;
        
        public GoblinBeastRiderIdleST(AEnemy enemy)
        {
            this._enemy = enemy;
        }
        
        public void Enter()
        {
            _goblinBeastRider = _enemy.gameObject.GetComponent<GoblinBeastRider>(); 
            _goblinBeastRider.goblinBeastRiderAnimator.SetAnimation(EntityMoves.Idle);
        }

        public void Execute()
        {
            
        }

        public void FixedExecute()
        {
            
        }

        public void Exit()
        {
            
        }
        
    }
    
       public class GoblinBeastRiderRunST : IState
    {
        private AEnemy _enemy;
        private GoblinBeastRider _goblinBeastRider;
        private float _keepDistance = 3f; 
        
        public GoblinBeastRiderRunST(AEnemy enemy)
        {
            this._enemy = enemy;
        }
        
        public void Enter()
        {
            _goblinBeastRider = _enemy.gameObject.GetComponent<GoblinBeastRider>(); 
            _goblinBeastRider.goblinBeastRiderAnimator.SetAnimation(EntityMoves.Walk);
            Vector2 targetPosition = GetTargetPosition();
            _enemy.rb2D.linearVelocity = (targetPosition - _enemy.rb2D.position).normalized * _enemy.enemyData.moveSpeed.Value;
        }

        public void FixedExecute()
        {
            Vector2 targetPosition = GetTargetPosition();
            _enemy.rb2D.linearVelocity = (targetPosition - _enemy.rb2D.position).normalized * _enemy.enemyData.moveSpeed.Value;
        }
        
        private Vector2 GetTargetPosition()
        {
            Vector2 playerPos = PlayerStatus.Instance.gameObject.transform.position;
            Vector2 enemyPos = _enemy.rb2D.position;
            
            float targetX;
            if (enemyPos.x < playerPos.x)
            {
                targetX = playerPos.x - _keepDistance;
            }
            else
            {
                targetX = playerPos.x + _keepDistance;
            }
            
            return new Vector2(targetX, playerPos.y);
        }
        
        public void Execute()
        {
            _goblinBeastRider.enemySkillCooldown.DecreaseCooldownAll(Time.deltaTime);
            _enemy.FacePlayer();
            if (_goblinBeastRider.enemySkillCooldown.CheckToUseSkill("Roar"))
            {
                _enemy.fsm.ChangeState("Roar");
            }
            if (Vector2.Distance(_enemy.rb2D.position, PlayerStatus.Instance.gameObject.transform.position) < 5&&_goblinBeastRider.enemySkillCooldown.CheckToUseSkill("Dash"))
            {
                _enemy.fsm.ChangeState("DashPrepare");
            }
        }
        public void Exit()
        {
        
        }
    }
    public class GoblinBeastRiderDashPrepareST:IState
    {
        private AEnemy _enemy;
        private float _startTime;
        private float[] _waitDuration = {0.5f,1f};
        private float _dashRange = 9f;
        private StaticLineIndicator _sli;
        private Vector3 _finalDestination;

        public GoblinBeastRiderDashPrepareST(AEnemy enemy)
        {
            _enemy = enemy;
        }

        public void Enter()
        {
            _startTime = Time.time;
            _enemy.rb2D.linearVelocity = Vector2.zero;
            _enemy.gameObject.GetComponent<GoblinBeastRider>().goblinBeastRiderAnimator.SetAnimation(EntityMoves.PrepareDash);
            var o = ObjectPoolManager.Instance.Get(ObjectBankManager.Instance.Get(ObjectCode.StaticIndicatorLine),IndicatorCanvas.Instance.canvasPrefab.transform);
            var finalDestination = (PlayerStatus.Instance.gameObject.transform.position-_enemy.gameObject.transform.position).normalized*_dashRange;
            _sli = o.GetComponent<StaticLineIndicator>();
            _sli.SetParent(_enemy.gameObject);
            _sli.SetTarget(_enemy.gameObject, _enemy.transform.position+finalDestination, new Vector3(0.5f, 0.5f, 0f));
            _sli.SetBaseSize(320,1000);
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
                _sli.SetBaseSize(320,1000);
                _enemy.FacePlayer();
            }

            if (Time.time - _startTime >= _waitDuration[1])
            {
                _sli.OnExit();
                _enemy.GetComponent<GoblinBeastRider>().finalDestination =  _finalDestination/_dashRange;
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
    public class GoblinBeastRiderDashST:IState
    {
        private AEnemy _enemy;
        private Vector2 _finalDestination;
        private float _dashSpeed = 12f;
        private float _dashRange = 9f;
        private AfterImageGenerator _generator;

        public GoblinBeastRiderDashST(AEnemy enemy)
        {
            _enemy = enemy;
        }

        public void Enter()
        {
            _enemy.rb2D.linearVelocity  = Vector2.zero;
            _enemy.gameObject.GetComponent<GoblinBeastRider>().goblinBeastRiderAnimator.SetAnimation(EntityMoves.Dash);
            var o = ObjectPoolManager.Instance.Get(ObjectBankManager.Instance.Get(ObjectCode.AfterImageGenerator),Vector2.zero,Vector3.zero);
            _generator = o.GetComponent<AfterImageGenerator>();
            _generator.StartSpawn(_enemy.gameObject,0.05f);
            var dir = _enemy.GetComponent<GoblinBeastRider>().finalDestination;
            _finalDestination =dir*_dashRange+_enemy.rb2D.position;
            _enemy.rb2D.linearVelocity  = dir*_dashSpeed;
            _enemy.enemyData.baseAttack.AddBuff(BuffType.Mul,3);
        }

        public void Execute()
        {
            if (Vector2.Distance(_enemy.rb2D.position, _finalDestination) <= 0.15f)
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
            _enemy.enemyData.baseAttack.AddBuff(BuffType.Mul,-3);
            ObjectPoolManager.Instance.Return(_generator.gameObject);
        }
        
    }
    
}