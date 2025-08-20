using System;
using Random = UnityEngine.Random;

[Serializable]
public class RandomRanged
{
    public float maxRange;
    public float minRange;

    public float GetRandomized()
    {
        return float.Parse($"{Random.Range(minRange, maxRange):F1}");
    }
    public int GetRandomizedAsInt()
    {
        return (int)Random.Range(minRange, maxRange);
    }
}
