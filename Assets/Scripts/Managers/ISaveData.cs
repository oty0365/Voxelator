using UnityEngine;

public interface ISaveData
{
    string ToJson();
    void FromJson(string json);
}
