using System;
using UnityEditor.Overlays;
using UnityEngine;

[Serializable]
public class PlayerStatUpgrade : ISaveData
{
    public int level;
    public int atk;
    public int def;
    public int hp;
    
    public string ToJson() => JsonUtility.ToJson(this);
    public void FromJson(string json) => JsonUtility.FromJsonOverwrite(json, this);
}

public class PlayerStatUpgradeInfo: MonoBehaviour,ISaveable
{
    public PlayerStatUpgrade upgrades;
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
            
            _saveManager.SaveData(GetSavePath(), upgrades);
        }
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
