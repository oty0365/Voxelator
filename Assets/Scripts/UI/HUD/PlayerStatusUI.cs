using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerStatusUI : MonoBehaviour,IEvent
{
    [SerializeField] private Slider playerExp;
    [SerializeField] private TextMeshProUGUI playerLevelText;
    [SerializeField] private TextMeshProUGUI playerHpText;
    [SerializeField] private TextMeshProUGUI playerAtkText;
    [SerializeField] private TextMeshProUGUI playerDefText;
    private PlayerStatus _playerStatus;
    
    public void SetMaxExp(float exp)
    {
        playerExp.maxValue = exp;
    }
    public void SetExp(float exp)
    {
        playerExp.value = exp;
    }

    public void SetLevel(int level)
    {
        playerLevelText.text = level.ToString();
    }

    public void SetAtk(float atk)
    {
        playerAtkText.text = $"{FormatStat(atk)}";
    }

    public void SetDef(float def)
    {
        playerDefText.text = $"{FormatStat(def)}";
    }

    public void SetHp(float hp,float maxHp)
    {
        playerHpText.text = $"<color=green>{FormatStat(hp)}</color>/{FormatStat(maxHp)}";
    }
    
    float FormatStat(float value)
    {
        if ((value * 100) % 1 != 0)
            return (float)Math.Round(value, 2);
        return value;
    }

    public void Subscribe()
    {
        _playerStatus= PlayerStatus.Instance;
        _playerStatus.playerHp.OnChanged += SetHp;
        _playerStatus.playerHp.OnChanged += CameraManager.Instance.OnHealthChange;
        _playerStatus.playerAtk.OnChanged += SetAtk;
        _playerStatus.playerDef.OnChanged += SetDef;
        _playerStatus.OnMaxExp += SetMaxExp;
        _playerStatus.OnExp += SetExp;
        _playerStatus.OnLevelUp += SetLevel;
    }

    public void Unsubscribe()
    {
        _playerStatus.playerHp.OnChanged -= SetHp;
        _playerStatus.playerHp.OnChanged -= CameraManager.Instance.OnHealthChange;
        _playerStatus.playerAtk.OnChanged -= SetAtk;
        _playerStatus.playerDef.OnChanged -= SetDef;
        _playerStatus.OnMaxExp -= SetMaxExp;
        _playerStatus.OnExp -= SetExp;
        _playerStatus.OnLevelUp -= SetLevel;
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
