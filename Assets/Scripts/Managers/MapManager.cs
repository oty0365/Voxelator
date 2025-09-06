using System;
using System.Collections.Generic;
using UnityEngine;

public enum MapCode
{
    Root,
    WindWildPlain
}
public class MapManager : SingletonMonoBehaviour<MapManager>
{
    [SerializeField] private MapSetsSO mapSetsSO;
    private GameObject _currentMap;
    private MapCode _currentMapCode;
    private Dictionary<MapCode, GameObject> _currentMapDict = new();

    protected override void Awake()
    {
        base.Awake();
        foreach (var m in mapSetsSO.set)
        {
            if (!_currentMapDict.ContainsKey(m.code))
            {
                _currentMapDict.Add(m.code,m.prefab);
            }
        }
        
    }
    public void ChangeMap(MapCode code)
    {
        _currentMapCode = code;
    }
    public void InstantiateMap()
    {
        if (_currentMap != null)
        {
            Destroy(_currentMap);
            _currentMap = null;
        }
        _currentMap = Instantiate(_currentMapDict[_currentMapCode], new Vector3(0, 0, 0), Quaternion.identity);
    }

    
}
