using System;
using System.Collections;
using UnityEngine;

public class PlayerStatus : SceneSingletonMonoBehaviour<PlayerStatus>,IEvent
{
    [Header("UI")]
    [SerializeField] private PlayerStatusUI playerStatusUI;
    [Header("무적상태인지 확인")]
    public bool isInfinite;
    [Header("플레이어 설정")]
    [SerializeField] private PlayerBasicStatusDataSO playerBasicStatusData;
    [SerializeField] private Collider2D collider2D;
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
        playerHp.SetMaxBuff(BuffType.Add,playerBasicStatusData.playerMaxHp); 
        playerHp.SetBuff(BuffType.Add,playerBasicStatusData.playerMaxHp);
        playerMoveSpeed.SetBuff(BuffType.Add,playerBasicStatusData.playerMoveSpeed);
        playerAtk.SetBuff(BuffType.Add,playerBasicStatusData.playerAtk);
        playerDef.SetBuff(BuffType.Add,playerBasicStatusData.playerDef);
        playerAttackSpeed.SetBuff(BuffType.Add,playerBasicStatusData.playerAttackSpeed);
        PlayerMaxExp = playerBasicStatusData.playerMaxExp;
        PlayerExp = 0;
        PlayerLevel = 1;
        playerSkillCooldown.SetMaxBuff(BuffType.Add,playerBasicStatusData.playerSkillCoolDown);
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
        collider2D.excludeLayers = layerMask;
        isInfinite = true;
        yield return new WaitForSeconds(time);
        isInfinite = false;
        collider2D.excludeLayers = originMask;
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
            collider2D.excludeLayers = originMask;
        }

        _infiniteTimeFlow = StartCoroutine(InfiniteTimeFlow(infiniteTime));
    }
    public void Subscribe()
    {
        playerHp.OnChanged += playerStatusUI.SetHp;
        playerAtk.OnChanged += playerStatusUI.SetAtk;
        playerDef.OnChanged += playerStatusUI.SetDef;
        OnMaxExp += playerStatusUI.SetMaxExp;
        OnExp += playerStatusUI.SetExp;
        OnLevelUp += playerStatusUI.SetLevel;
        EventManager.Instance.AddListener(EventKey.OnPlayerHit,new Action<GameObject>(OnDamage));
    }
    public void Unsubscribe()
    {
        playerHp.OnChanged -= playerStatusUI.SetHp;
        playerAtk.OnChanged -= playerStatusUI.SetAtk;
        playerDef.OnChanged -= playerStatusUI.SetDef;
        OnMaxExp -= playerStatusUI.SetMaxExp;
        OnExp -= playerStatusUI.SetExp;
        OnLevelUp -= playerStatusUI.SetLevel;
        EventManager.Instance.RemoveListener(EventKey.OnPlayerHit,new Action<GameObject>(OnDamage));
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
            CammeraManager.Instance.ShakeCamera(damageData.damage/(9+damageData.damage),0.5f);
            playerHp.SetBuff(BuffType.Add,-realDamage);   
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
