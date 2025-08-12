using System.Collections.Generic;
using UnityEngine;

public class ObjectBankManager : SceneSingletonMonoBehaviour<ObjectBankManager>
{
    [SerializeField] private ObjectBankSO objectBankSO; 
    private Dictionary<string, GameObject> bank = new();

    private void Start()
    {
        foreach(var i in objectBankSO.objectBankSet)
        {
            bank.Add(i.name, i.data);
        }
    }
    public GameObject Get(string key)
    {
        if (bank.ContainsKey(key))
        {
            return bank[key];
        }
        return null;
    }
}
