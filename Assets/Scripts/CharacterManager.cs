using System.Collections.Generic;
using UnityEngine;

public class CharacterManager : SceneSingletonMonoBehaviour<CharacterManager>
{
    [SerializeField] private CharacterDataArraySO characterDataArray;
    private Dictionary<string, CharacterDataSO> _characterDataDictionary = new();
    private void Start()
    {
        foreach (var c in characterDataArray.characterData)
        {
            _characterDataDictionary.Add(c.code, c);
        }
    }
    public CharacterDataSO GetData(string code)
    {
        if (_characterDataDictionary.ContainsKey(code))
        {
            return _characterDataDictionary[code];
        }
        return null;
    }
}
