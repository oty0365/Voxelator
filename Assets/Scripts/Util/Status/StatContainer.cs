using System.Collections.Generic;
using UnityEngine;

public enum StatusCode
{
    Hp,
    Def,
    Atk,
    MoveSpeed,
}

public class StatContainer : MonoBehaviour
{
    private Dictionary<StatusCode,Stat<float>> _statsDict = new();

    public void AddStat(StatusCode status, Stat<float> stat)
    {
        if (_statsDict.ContainsKey(status))
        {
            DeleteStat(status);
        }
        _statsDict.Add(status,stat);
    }
    public void DeleteStat(StatusCode status)
    {
        _statsDict.Remove(status);
    }
    public T GetStat<T>(StatusCode code) where T : class
    {
        if (_statsDict.TryGetValue(code, out var stat))
        {
            return stat as T;
        }
        return null;
    }
}
