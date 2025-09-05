using System;
using UnityEngine;

[Serializable]
public class MapSet
{
    public MapCode code;
    public GameObject prefab;
}

[CreateAssetMenu(fileName = "MapSetsSO", menuName = "Scriptable Objects/MapSetsSO")]
public class MapSetsSO : ScriptableObject
{
    public MapSet[] set;
}
