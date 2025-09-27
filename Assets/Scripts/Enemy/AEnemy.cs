using System;
using System.Collections;
using UnityEngine;

public class RunTimeEnemyData
{
    public LimitedStat health = new();
    public UnlimitedStat baseAttack = new();
    public UnlimitedStat baseDefense = new(); 
    public UnlimitedStat moveSpeed = new();
    public int expDrop;

}

public abstract class AEnemy : MonoBehaviour,ILifeSycler
{
    [SerializeField] private EnemyDataSO baseEnemyData;
    [SerializeField] private StatContainer statContainer;
    public CharacterType characterType;
    public RunTimeEnemyData enemyData = new();
    public Rigidbody2D rb2D;
    public Fsm fsm;
    public SpriteRenderer sr;
    public event Action OnDeath;
    protected MaterialPropertyBlock _metProps;
    protected Coroutine _currentHitFlow;

    public virtual void Initialize()
    {
        EnemySpawn.Instance.UpLoadToList(gameObject);
        _metProps = new MaterialPropertyBlock();
        sr.GetPropertyBlock(_metProps);
        _metProps.SetFloat("_Progress", 0);
        sr.SetPropertyBlock(_metProps);
        
        //.health.MaxValue=;
        //enemyData.health.Value=enemyData.health.MaxValue;
        //enemyData.baseAttack.Value = baseEnemyData.baseAttack.GetRandomized();
        //enemyData.baseDefense.Value = baseEnemyData.baseDefense.GetRandomized();
        //enemyData.moveSpeed.Value = baseEnemyData.moveSpeed;
        characterType.EntityType = characterType.entityType;
        
        
        enemyData.expDrop = baseEnemyData.expDrop.GetRandomizedAsInt()+(int)Mathf.Round(TimeManager.Instance.gameTime*EnemySpawn.Instance.spawnSettings.expIncreaseInterval);
        enemyData.health.AddMaxBuff(BuffType.Add,baseEnemyData.health.GetRandomized()+TimeManager.Instance.gameTime*EnemySpawn.Instance.spawnSettings.healthIncreaseInterval);
        enemyData.health.AddBuff(BuffType.Add,enemyData.health.MaxValue);
        enemyData.baseAttack.AddBuff(BuffType.Add,baseEnemyData.baseAttack.GetRandomized()+TimeManager.Instance.gameTime*EnemySpawn.Instance.spawnSettings.attackIncreaseInterval);
        enemyData.baseDefense.AddBuff(BuffType.Add,baseEnemyData.baseDefense.GetRandomized());
        enemyData.moveSpeed.SetBuff(BuffType.Add,baseEnemyData.moveSpeed);
        
        statContainer.AddStat(StatusCode.Hp,enemyData.health);
        statContainer.AddStat(StatusCode.Def,enemyData.baseDefense);
        statContainer.AddStat(StatusCode.MoveSpeed,enemyData.moveSpeed);
        statContainer.AddStat(StatusCode.Atk,enemyData.baseAttack);
        
        
    }
    
    public void FacePlayer()
    {
        var playerPos = PlayerStatus.Instance.transform.position;
        if (gameObject.transform.position.x > playerPos.x)
        {
            sr.flipX = true;
        }
        else
        {
            sr.flipX = false;
        }
    }

    private IEnumerator HitFlow()
    {
        for (float i = 0f; i < 1.2f; i += Time.deltaTime * 10f)
        {
            sr.GetPropertyBlock(_metProps);
            _metProps.SetFloat("_Progress", i);
            sr.SetPropertyBlock(_metProps);
            yield return null;
        }

        sr.GetPropertyBlock(_metProps);
        _metProps.SetFloat("_Progress", 1);
        sr.SetPropertyBlock(_metProps);

        for (float i = 1.2f; i > 0f; i -= Time.deltaTime * 10f)
        {
            sr.GetPropertyBlock(_metProps);
            _metProps.SetFloat("_Progress", i);
            sr.SetPropertyBlock(_metProps);
            yield return null;
        }

        sr.GetPropertyBlock(_metProps);
        _metProps.SetFloat("_Progress", 0);
        sr.SetPropertyBlock(_metProps);
    }



    public virtual void Drop()
    {
        var a = ObjectPoolManager.Instance.Get(ObjectBankManager.Instance.Get(ObjectCode.Exp), transform.position, new Vector3(0, 0, 45));
        var exp = a.GetComponent<ExpGiver>();
        exp.expAmount = enemyData.expDrop;
        exp.SetExpColor();
    }

    private void OnDisable()
    {
        OnDeath?.Invoke();
    }

    public virtual void OnHit(DamageData damage)
    {
        var defenceValue = 9f;
        var realDamage=damage.damage - damage.damage * (enemyData.baseDefense.Value / (enemyData.baseDefense.Value + defenceValue));
        if (enemyData.baseDefense.Value <= 0)
        {
            realDamage = damage.damage;
        }

        if (realDamage > 0)
        {
            var hit = ObjectPoolManager.Instance.Get(ObjectBankManager.Instance.Get(ObjectCode.HitParticle), transform.position, new Vector3(-90, 0, 0)); 
            var hitObj = hit.GetComponent<ParticleObject>();
            var hitPrt = hitObj.prt.main;
            hitPrt.startColor = characterType.GetColor();
        }
        enemyData.health.Value -= realDamage;
        if (_currentHitFlow != null)
        {
            StopCoroutine(_currentHitFlow);
        }
        if (gameObject.activeSelf)
        {
            _currentHitFlow = StartCoroutine(HitFlow());
        }

        if (enemyData.health.Value <= 0)
        {
            Death();
        }
    }

    public virtual void Death()
    {
        Drop();
        Vanish();
    }

    public virtual void Vanish()
    {
        enemyData.health.SetMaxBuff(BuffType.Add,0);
        enemyData.health.SetBuff(BuffType.Add,0);
        enemyData.baseAttack.SetBuff(BuffType.Add,0);
        enemyData.baseDefense.SetBuff(BuffType.Add,0);
        enemyData.moveSpeed.SetBuff(BuffType.Add,0);
        
        statContainer.DeleteStat(StatusCode.Hp);
        statContainer.DeleteStat(StatusCode.Def);
        statContainer.DeleteStat(StatusCode.MoveSpeed);
        statContainer.DeleteStat(StatusCode.Atk);
        EnemySpawn.Instance.RemoveInList(gameObject);
        ObjectPoolManager.Instance.Return(gameObject);
    }
}
public class EnemyMoveTowardsST : IState
{
    private AEnemy _enemy;
        
    public EnemyMoveTowardsST(AEnemy enemy)
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
public class EnemyMoveTowardsNonFlipST : IState
{
    private AEnemy _enemy;
        
    public EnemyMoveTowardsNonFlipST(AEnemy enemy)
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
        
    }
    public void Exit()
    {
        
    }
}

