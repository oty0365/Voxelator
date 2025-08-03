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
    public event Action<float> OnAtk;
    public event Action<float> OnDef;
    public event Action<float,float> OnHp;

    [SerializeField] private PlayerBasicStatusData playerBasicStatusData;
    [SerializeField] private Collider2D collider2D;
    [SerializeField] private LayerMask layerMask;
    [SerializeField] private LayerMask originMask;
    [SerializeField] private GameObject playerHitParticle;
    [SerializeField] private GameObject gameOverPanel;
    
    private float _playerMaxHp;
    private float _playerHp;
    private float _playerDef;
    private float _playerAtk;
    private float _playerMoveSpeed;
    private float _playerAttackSpeed;
    private float _playerExp;
    private int _playerLevel;
    private float _playerMaxExp;
    private int _playerBulletCount;
    private Coroutine _infiniteTimeFlow;

    public float PlayerMaxHp
    {
        get=>_playerMaxHp;
        private set
        {
            if (value != _playerMaxHp)
            {
                _playerMaxHp = value;
            }

            if (value < PlayerHp)
            {
                PlayerHp = value;
            }
            OnHp?.Invoke(PlayerHp,_playerMaxHp);
        }
    }
    
    public float PlayerHp
    {
        get => _playerHp;
        private set
        {
            if (value != _playerHp)
            {
                if (value < _playerHp)
                {
                    ObjectPooler.Instance.Get(playerHitParticle,gameObject.transform.position,new Vector3(-90,0,0));
                    //SoundManager.Instance.PlaySFX("PlayerHit");
                    CammeraManager.Instance.ShakeCamera(0.4f,0.2f);
                }
                _playerHp = value;
            }

            if (value <= 0)
            {
                _playerHp = 0;
                Time.timeScale = 0;
                gameOverPanel.SetActive(true);
            }

            if (value > PlayerMaxHp)
            {
                _playerHp = PlayerMaxHp;
            }
            OnHp?.Invoke(_playerHp, _playerMaxHp);
        }
    }

    public float PlayerDef
    {
        get => _playerDef;
        set
        {
            if (_playerDef != value)
            {
                _playerDef = value;
            }
            OnDef?.Invoke(_playerDef);
        }
    }

    public float PlayerAtk
    {
        get => _playerAtk;
        private set
        {
                _playerAtk = value;
                OnAtk?.Invoke(_playerAtk);
            
        }
    }

    public float PlayerMoveSpeed
    {
        get => _playerMoveSpeed;
        private set
        {
            if (_playerMoveSpeed != value)
            {
                _playerMoveSpeed = value;
            }
        }
    }

    public float PlayerAttackSpeed
    {
        get => _playerAttackSpeed;
        private set
        {
            if (_playerAttackSpeed != value)
            {
                _playerAttackSpeed = value;
            }
        }
    }

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
            PlayerHp += 5f;
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

    private float CalculateExpRequirementLinear(int level)
    {
        float baseExp = 100f;
        float increment = 50f;
        return baseExp + (increment * (level - 1));
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
    private void Start()
    {
        OnMaxExp += PlayerStatusUI.Instance.SetMaxExp;
        OnExp += PlayerStatusUI.Instance.SetExp;
        OnLevelUp += PlayerStatusUI.Instance.SetLevel;
        OnAtk += PlayerStatusUI.Instance.SetAtk;
        OnDef += PlayerStatusUI.Instance.SetDef;
        OnHp += PlayerStatusUI.Instance.SetHp;
        
        PlayerMaxHp = playerBasicStatusData.playerMaxHp;
        PlayerHp = PlayerMaxHp;
        PlayerMoveSpeed = playerBasicStatusData.playerMoveSpeed;
        PlayerAtk = playerBasicStatusData.playerAtk;
        PlayerDef = playerBasicStatusData.playerDef;
        PlayerMaxExp = playerBasicStatusData.playerMaxExp;
        PlayerAttackSpeed = playerBasicStatusData.playerAttackSpeed;
        PlayerExp = 0;
        PlayerLevel = 1;
    }

    public void SetExp(float exp)
    {
        PlayerExp = exp;
    }

    public void SetMaxHp(float hp)
    {
        PlayerMaxHp = hp;
    }

    public void SetHp(float hp)
    {
        PlayerHp = hp;
    }

    public void SetAtk(float atk)
    {
        PlayerAtk = atk;
    }

    public void SetAtkSpeed(float atkSpeed)
    {
        PlayerAttackSpeed = atkSpeed;
    }

    public void SetDef(float def)
    {
        PlayerDef = def;
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
        var realDamage = damage - PlayerDef;
        if (realDamage > 0)
        {
            SetHp(PlayerHp - realDamage);
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
