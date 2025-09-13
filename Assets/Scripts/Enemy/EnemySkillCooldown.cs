using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class EnemySkillCooldown
{
    private Dictionary<string, float> _coolDownSetData = new();
    private Dictionary<string, float> _runtimeCooldownData = new();

    public void AddPattern(string name, float coolDown)
    {
        if (!_coolDownSetData.ContainsKey(name))
        {
            _coolDownSetData.Add(name, coolDown);
            _runtimeCooldownData.Add(name,0);
        }
    }

    public void RemovePattern(string name)
    {
        if (_coolDownSetData.ContainsKey(name))
        {
            _coolDownSetData.Remove(name);
            _runtimeCooldownData.Remove(name);
        }
    }

    public float GetCooldown(string name)
    {
        return _runtimeCooldownData[name];
    }
    
    public void DecreaseCooldown(string name,float amount)
    {
        _runtimeCooldownData[name] -= amount;
    }

    public void DecreaseCooldownAll(float amount)
    {
        var keys = _runtimeCooldownData.Keys.ToList();
        foreach (var key in keys)
        {
            _runtimeCooldownData[key] -= amount;
        }
    }


    public bool CheckToUseSkill(string name)
    {
        if (_runtimeCooldownData.ContainsKey(name)&&_runtimeCooldownData[name]<=0)
        {
            _runtimeCooldownData[name] = _coolDownSetData[name];
            return true;
        }
        return false;
    }

    public void SetSkillCool(string name,float amount)
    {
        if(_runtimeCooldownData.ContainsKey(name))
        {
            _runtimeCooldownData[name] = amount;
        }
    }
    public void ReSetSkillCool(string name)
    {
        if(_runtimeCooldownData.ContainsKey(name))
        {
            _runtimeCooldownData[name] = _coolDownSetData[name];
        }
    }

}
