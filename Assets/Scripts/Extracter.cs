using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEditor;
using UnityEngine;

public class Extracter : HalfSingleMono<Extracter> 
{
    public Dictionary<string, Stat<float>> stats = new();

    public void UpLoadStats()
    {
        stats.Add("ATK", PlayerStatus.Instance.playerAtk);
        stats.Add("DEF", PlayerStatus.Instance.playerDef);
        stats.Add("HP", PlayerStatus.Instance.playerHp);
        stats.Add("COOL", PlayerStatus.Instance.playerSkillCooldown);
    }

    public float ParseFloat(string expression)
    {
        if (string.IsNullOrEmpty(expression))
            return 0f;

        foreach (var stat in stats)
        {

            string maxKey = "MAX" + stat.Key;
            if (stat.Value is LimitedStat limitedStat)
            {
                expression = Regex.Replace(expression, @"\b" + maxKey + @"\b",
                                         limitedStat.MaxValue.ToString(), RegexOptions.IgnoreCase);
            }
        }
        foreach (var stat in stats)
        {
            expression = Regex.Replace(expression, @"\b" + stat.Key + @"\b",stat.Value.Value.ToString(), RegexOptions.IgnoreCase);
        }
        try
        {
            return Convert.ToSingle(new DataTable().Compute(expression, null));
        }
        catch
        {
            return 0f;
        }
    }
}
