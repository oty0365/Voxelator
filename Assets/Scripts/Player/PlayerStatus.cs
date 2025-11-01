using System;
using System.Collections;
using UnityEngine;

public class PlayerStatus : SceneSingletonMonoBehaviour<PlayerStatus>,IEvent
{
//    [Header("UI")]
//    [SerializeField] private PlayerStatusUI playerStatusUI;
    [Header("무적상태인지 확인")]
    public bool isInfinite;
    [Header("플레이어 설정")]
    [SerializeField] private PlayerBasicStatusDataSO playerBasicStatusData;
    [SerializeField] private Collider2D col2D;
    [SerializeField] private LayerMask layerMask;
    [SerializeField] private LayerMask originMask;
    [SerializeField] private StatContainer statContainer;
    
    public LimitedStat playerHp = new();
    public UnlimitedStat playerDef = new();
    public UnlimitedStat playerAtk = new();
    public UnlimitedStat playerMoveSpeed = new();
    public UnlimitedStat playerAttackSpeed = new();
    public LimitedStat playerSkillCooldown = new();
    public event Action<float> OnMaxExp;
    public event Action<int> OnLevelUp;
    public event Action<float> OnExp;

    private float _playerExp;
    private int _playerLevel;
    private float _playerMaxExp;
    private int _playerBulletCount;
    private Coroutine _infiniteTimeFlow;

    public float PlayerMaxExp
    {
        get => _playerMaxExp;
        private set
        {
            if (_playerMaxExp != value)
            {
                _playerMaxExp = value;
                OnMaxExp?.Invoke(_playerMaxExp);
            }
        }
    }

    public float PlayerExp
    {
        get => _playerExp;
        set
        {
            if (value >= 0)
            {
                float delta = value - _playerExp;
                AddExp(delta);
            }
        }
    }

    public void AddExp(float expGained)
    {
        if (expGained <= 0) return;

        _playerExp += expGained;

        if (_playerExp >= PlayerMaxExp)
        {
            HandleLevelUpLogic();
        }
        else
        {
            OnExp?.Invoke(_playerExp);
        }
    }

    private void HandleLevelUpLogic()
    {
        while (_playerExp >= PlayerMaxExp)
        {
            float currentMaxExp = PlayerMaxExp;
            _playerExp -= currentMaxExp;
            PlayerLevel++;
            PlayerMaxExp = CalculateExpRequirement(PlayerLevel);
        }
    
        OnExp?.Invoke(_playerExp);
    }

    private float CalculateExpRequirement(int level)
    {
        float baseExp = 100f;
        float multiplier = 1.2f;
        return baseExp * Mathf.Pow(multiplier, level - 1);
    }

    public int PlayerLevel
    {
        get => _playerLevel;
        private set
        {
            if (_playerLevel != value)
            {
                _playerLevel = value;
                OnLevelUp?.Invoke(_playerLevel);
            }
        }
    }
    
    public int PlayerBulletCount
    {
        get => _playerBulletCount;
        private set
        {
            if (_playerBulletCount != value)
            {
                _playerBulletCount = value;
            }
        }
    }

    protected override void Awake()
    {
        base.Awake();
        PlayerBulletCount = playerBasicStatusData.playerBulletCount;
    }
    public void ResetStatus()
    {
        playerHp.AddMaxBuff(BuffType.Add,playerBasicStatusData.playerMaxHp); 
        playerHp.AddBuff(BuffType.Add,playerBasicStatusData.playerMaxHp);
        playerMoveSpeed.AddBuff(BuffType.Add,playerBasicStatusData.playerMoveSpeed);
        playerAtk.AddBuff(BuffType.Add,playerBasicStatusData.playerAtk);
        playerDef.AddBuff(BuffType.Add,playerBasicStatusData.playerDef);
        
        playerAtk.AddBuff(BuffType.Add,RuntimeUpgradeStatManager.Instance.GetAtk());
        playerDef.AddBuff(BuffType.Add,RuntimeUpgradeStatManager.Instance.GetDef());
        playerHp.AddMaxBuff(BuffType.Add,RuntimeUpgradeStatManager.Instance.GetHp()); 
        playerHp.AddBuff(BuffType.Add,RuntimeUpgradeStatManager.Instance.GetHp());
        
        playerAttackSpeed.AddBuff(BuffType.Add,playerBasicStatusData.playerAttackSpeed);
        PlayerMaxExp = playerBasicStatusData.playerMaxExp;
        PlayerExp = 0;
        PlayerLevel = 1;
        playerSkillCooldown.AddMaxBuff(BuffType.Add,playerBasicStatusData.playerSkillCoolDown);
        playerSkillCooldown.Value = 0;
        statContainer.AddStat(StatusCode.Atk,playerAtk);
        statContainer.AddStat(StatusCode.Def,playerDef);
        statContainer.AddStat(StatusCode.Hp,playerHp);
        statContainer.AddStat(StatusCode.MoveSpeed,playerMoveSpeed);
    }

    public void SetExp(float exp)
    {
        PlayerExp = exp;
    }
    private IEnumerator InfiniteTimeFlow(float time)
    {
        col2D.excludeLayers = layerMask;
        isInfinite = true;
        yield return new WaitForSeconds(time);
        isInfinite = false;
        col2D.excludeLayers = originMask;
    }

    public void GetDamage(float damage, float infiniteTime)
    {
        var realDamage = damage - playerDef.Value;
        if (realDamage > 0)
        {
            //statContainer.SetStat(StatusCode.Hp,playerHp.Value - realDamage);
        }
        if (_infiniteTimeFlow != null)
        {
            StopCoroutine(_infiniteTimeFlow);
            isInfinite =  false;
            col2D.excludeLayers = originMask;
        }

        _infiniteTimeFlow = StartCoroutine(InfiniteTimeFlow(infiniteTime));
    }
    public void Subscribe()
    {
        EventManager.Instance.AddListener(EventKey.OnPlayerHit,new Action<GameObject>(OnDamage));
    }
    public void Unsubscribe()
    {
        if (EventManager.Instance != null)
        {
            EventManager.Instance.RemoveListener(EventKey.OnPlayerHit,new Action<GameObject>(OnDamage));
        }
    }

    public void OnDamage(GameObject damager)
    {
        var damagerObject = damager.GetComponent<Damager>();
        var damageData =damager.GetComponent<IDamager>().GetDamage(damagerObject.parent.GetComponent<StatContainer>().GetStat<UnlimitedStat>(StatusCode.Atk).Value);
        var defenceValue = 9f;
        var realDamage=damageData.damage - damageData.damage * (playerDef.Value / (playerDef.Value + defenceValue));
        if (playerDef.Value <= 0)
        {
            realDamage = damageData.damage;
        }
        if (realDamage > 0)
        {
            CameraManager.Instance.ShakeCamera(damageData.damage/(9+damageData.damage),0.5f);
            playerHp.AddBuff(BuffType.Add,-realDamage);   
        }
    }
    private void OnEnable()
    {
        Subscribe();
    }
    private void OnDisable()
    {
        Unsubscribe();
    }
}
