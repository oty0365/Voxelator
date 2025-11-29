using System;
using UnityEngine;

[Serializable]
public class PlayerStatUpgradeInfo : ISaveData
{
    public int level;
    public int atk;
    public int def;
    public int hp;
    public int upgradePoint;
    
    public string ToJson() => JsonUtility.ToJson(this);
    public void FromJson(string json) => JsonUtility.FromJsonOverwrite(json, this);
}

public class PlayerStatUpgrade: MonoBehaviour,ISaveAble,IDataGetSetter<PlayerStatUpgradeInfo>
{
    public PlayerStatUpgradeInfo upgrades;
    public string GetSavePath()=> "PlayerStatUpgrade";
    private SaveManager _saveManager;

    public void OnSave()
    {
        _saveManager.SaveData(GetSavePath(),upgrades);
    }

    public void OnLoad()
    {
        if (_saveManager.HasData(GetSavePath()))
        {
            upgrades.FromJson(_saveManager.LoadData(GetSavePath()));
        }
        else
        {
            upgrades.level = 0;
            upgrades.atk = 0;
            upgrades.def = 0;
            upgrades.hp = 0;
            upgrades.upgradePoint = 0;
            _saveManager.SaveData(GetSavePath(), upgrades);
        }
    }

    public void Get(PlayerStatUpgradeInfo data)
    {
        data.atk = upgrades.atk;
        data.level = upgrades.level;
        data.def = upgrades.def;
        data.hp = upgrades.hp;
        data.upgradePoint = upgrades.upgradePoint;
    }
    
    public void Set(PlayerStatUpgradeInfo data)
    {
        upgrades = data;
    }

    public void OnRemove()
    {
        _saveManager.RemoveData(GetSavePath());
    }

    void Start()
    {
        _saveManager = SaveManager.Instance;
        OnLoad();
    }
}
