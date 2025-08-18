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

public abstract class AEnemy : MonoBehaviour,IDamageStat
{
    [SerializeField] private EnemyDataSO baseEnemyData;
    public RunTimeEnemyData enemyData = new();
    public Rigidbody2D rb2D;
    public Fsm fsm;
    public SpriteRenderer sr;
    protected MaterialPropertyBlock _metProps;
    protected Coroutine _currentHitFlow;
    //피격 이후 효과는 나중에 이펙터같은 클래스 하나 만들어서 해결하는 방식으로 하겠습니다
    //public GameObject hitEffect;

    public virtual void Initialize()
    {
        _metProps = new MaterialPropertyBlock();
        sr.GetPropertyBlock(_metProps);
        _metProps.SetFloat("_Progress", 0);
        sr.SetPropertyBlock(_metProps);
        
        enemyData.health.MaxValue=baseEnemyData.health.GetRandomized();
        enemyData.health.Value=enemyData.health.MaxValue;
        enemyData.baseAttack.Value = baseEnemyData.baseAttack.GetRandomized();
        enemyData.baseDefense.Value = baseEnemyData.baseDefense.GetRandomized();
        enemyData.expDrop = baseEnemyData.expDrop.GetRandomizedAsInt();
        enemyData.moveSpeed.Value = baseEnemyData.moveSpeed;
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

    public float GetStat()
    {
        return enemyData.baseAttack.Value;
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
        var a = ObjectPoolManager.Instance.Get(ExpBank.Instance.exp, transform.position, new Vector3(0, 0, 45));
        a.GetComponent<ExpGiver>().expAmount = enemyData.expDrop;
    }
    //몬스터 피격판정은 플레이어 피격판정 이후에 만들 것이고 플레이어와 다르게 다중히트 허용이니 그것도 신경 쓸 것입니다.
    /*
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Weapon"))
        {
            Hit(other.gameObject);
        }

        if (other.CompareTag("Player"))
        {
            if (!PlayerStatus.Instance.isInfinite)
            {
                PlayerStatus.Instance.GetDamage(hitModule.damage, hitModule.infiniteTime);
            }
        }
    }
    public virtual void Hit(GameObject caster)
    {
        float totalDamage = caster.GetComponent<WeaponModule>().damage + PlayerStatus.Instance.PlayerAtk;
        CurrentHp -= totalDamage;

        ObjectPooler.Instance.Get(hitEffect, transform.position, new Vector3(-90, 0, 0));
        SoundManager.Instance.PlaySFX("Hit");
        if (_currentHitFlow != null)
        {
            StopCoroutine(_currentHitFlow);
        }

        if (gameObject.activeSelf)
        {
            _currentHitFlow = StartCoroutine(HitFlow());
        }
    }*/

    public virtual void Death()
    {
        ObjectPoolManager.Instance.Return(gameObject);
    }
}
public class EnemyMoveTowards : IState
{
    private AEnemy _enemy;
        
    public EnemyMoveTowards(AEnemy enemy)
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
public class EnemyMoveTowardsNonFlip : IState
{
    private AEnemy _enemy;
        
    public EnemyMoveTowardsNonFlip(AEnemy enemy)
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

