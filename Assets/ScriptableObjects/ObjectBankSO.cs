using System;
using UnityEngine;

[Serializable]
public class ObjectBankSet
{
    public string name;
    public GameObject data;
}

[CreateAssetMenu(fileName = "ObjectBankSO", menuName = "Scriptable Objects/ObjectBankSO")]
public class ObjectBankSO : ScriptableObject
{
    public ObjectBankSet[] objectBankSet;
}
