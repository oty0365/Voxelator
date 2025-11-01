using UnityEngine;

[CreateAssetMenu(fileName = "MapUIData", menuName = "Game/MapUIData")]
public class MapUIDataSO : ScriptableObject
{
    public string mapName;
    public Sprite mapImage;
    public MapCode mapCode;
}