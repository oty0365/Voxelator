using System.Collections.Generic;
using UnityEngine;

public class EffectContainer : MonoBehaviour
{
    private HashSet<EffectType> currentEffects = new ();

    public bool HasEffect(EffectType effectType)
    {
        return currentEffects.Contains(effectType);
    }

    public void AddEffect(EffectType effectType)
    {
        if (!currentEffects.Contains(effectType))
        {
            currentEffects.Add(effectType);
        }
    }

    public void RemoveEffect(EffectType effectType)
    {
        if (currentEffects.Contains(effectType))
        {
            currentEffects.Remove(effectType);
        }
    }
}
