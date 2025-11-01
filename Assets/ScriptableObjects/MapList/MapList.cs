using UnityEngine;

[CreateAssetMenu(fileName = "MapList", menuName = "Game/MapList")]
public class MapListSO : ScriptableObject
{
    public MapUIDataSO[] maps;
}