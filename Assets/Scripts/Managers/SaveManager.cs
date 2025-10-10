using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class SaveManager : SceneSingletonMonoBehaviour<SaveManager>
{

    [SerializeField] private string saveDirectory = "SaveData";
    private string _savePath => Path.Combine(Application.persistentDataPath, saveDirectory);
    /*private void Start()
    {
        Debug.Log(_savePath);
    }*/

    public void SaveData(string dataKey, ISaveData saveData)
    {
        if (!Directory.Exists(_savePath))
        {
            Directory.CreateDirectory(_savePath);
        }

        var path = Path.Combine(_savePath, dataKey + ".json");
        File.WriteAllText(path, JsonUtility.ToJson(saveData, true));
    }

    public string LoadData(string dataKey)
    {
        var path = Path.Combine(_savePath, dataKey+".json");
        if (File.Exists(path))
        {
            string json = File.ReadAllText(path);
            return json;
        }
        return null;
    }

    public void RemoveData(string dataKey)
    {
        var path = Path.Combine(_savePath, dataKey+".json");
        if (File.Exists(path))
        {
            File.Delete(path);
        }
    }

    public bool HasData(string dataKey)
    {
        var path = Path.Combine(_savePath, dataKey+".json");
        return File.Exists(path);
    }
    
}