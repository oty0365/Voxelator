using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using UnityEngine;

public class PlayerStatus : HalfSingleMono<PlayerStatus>
{
    public bool isInfinite;
    public event Action<float> OnMaxExp;
    public event Action<int> OnLevelUp;
    public event Action<float> OnExp;

    [SerializeField] private PlayerBasicStatusData playerBasicStatusData;
    [SerializeField] private Collider2D collider2D;
    [SerializeField] private LayerMask layerMask;
    [SerializeField] private LayerMask originMask;
    [SerializeField] private GameObject playerHitParticle;
    
    public LimitedStat playerHp = new();
    public UnlimitedStat playerDef = new();
    public UnlimitedStat playerAtk = new();
    public UnlimitedStat playerMoveSpeed = new();
    public UnlimitedStat playerAttackSpeed = new();
    public LimitedStat playerSkillCooldown = new();

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
        playerHp.OnChanged += PlayerStatusUI.Instance.SetHp;
        playerAtk.OnChanged += PlayerStatusUI.Instance.SetAtk;
        playerDef.OnChanged += PlayerStatusUI.Instance.SetDef;
        OnMaxExp += PlayerStatusUI.Instance.SetMaxExp;
        OnExp += PlayerStatusUI.Instance.SetExp;
        OnLevelUp += PlayerStatusUI.Instance.SetLevel;


        playerHp.MaxValue = playerBasicStatusData.playerMaxHp;
        playerHp.Value = playerHp.MaxValue;
        playerMoveSpeed.Value = playerBasicStatusData.playerMoveSpeed;
        playerAtk.Value = playerBasicStatusData.playerAtk;
        playerDef.Value = playerBasicStatusData.playerDef;
        playerAttackSpeed.Value = playerBasicStatusData.playerAttackSpeed;
        PlayerMaxExp = playerBasicStatusData.playerMaxExp;
        PlayerExp = 0;
        PlayerLevel = 1;
        playerSkillCooldown.MaxValue = 100;
        playerSkillCooldown.Value = 0;
    }

    public void SetExp(float exp)
    {
        PlayerExp = exp;
    }

    public void SetMaxHp(float hp)
    {
        playerHp.MaxValue = hp;
    }

    public void SetHp(float hp)
    {
        playerHp.Value = hp;
    }

    public void SetAtk(float atk)
    {
        playerAtk.Value = atk;
    }

    public void SetAtkSpeed(float atkSpeed)
    {
        playerAttackSpeed.Value = atkSpeed;
    }

    public void SetDef(float def)
    {
        playerDef.Value = def;
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
            SetHp(playerHp.Value - realDamage);
        }
        if (_infiniteTimeFlow != null)
        {
            StopCoroutine(_infiniteTimeFlow);
            isInfinite =  false;
            collider2D.excludeLayers = originMask;
        }

        _infiniteTimeFlow = StartCoroutine(InfiniteTimeFlow(infiniteTime));
    }
}
