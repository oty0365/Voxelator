using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class PlayerCharacterInfo : ISaveData
{
    public List<string> characterNames = new();
    
    public string ToJson() => JsonUtility.ToJson(this);
    public void FromJson(string json) => JsonUtility.FromJsonOverwrite(json, this);
}

public class PlayerCharacter : MonoBehaviour,ISaveable,IDataGetSetter<PlayerCharacterInfo>
{
    public PlayerCharacterInfo character;
    public string GetSavePath()=> "PlayerCharacter";
    private SaveManager _saveManager;

    public void OnSave()
    {
        _saveManager.SaveData(GetSavePath(),character);
    }

    public void OnLoad()
    {
        if (_saveManager.HasData(GetSavePath()))
        {
            character.FromJson(_saveManager.LoadData(GetSavePath()));
        }
        else
        {
            character.characterNames.Add("Knight");
            _saveManager.SaveData(GetSavePath(), character);
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

    public void Get(PlayerCharacterInfo data)
    {
        data.characterNames = character.characterNames;
    }
    
    public void Set(PlayerCharacterInfo data)
    {
        character.characterNames = data.characterNames;
    }
    
}
