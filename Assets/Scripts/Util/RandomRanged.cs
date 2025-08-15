using System;
using Random = UnityEngine.Random;

[Serializable]
public class RandomRanged
{
    public float maxRange;
    public float minRange;

    public float GetRandomized()
    {
        return Random.Range(minRange, maxRange);
    }
    public int GetRandomizedAsInt()
    {
        return (int)Random.Range(minRange, maxRange);
    }
}
