using System;
using UnityEngine;

public enum ObjectCode
{
    WeaponCore,
    Exp,
    HitParticle,
    InteractionButton,
    StaticIndicatorArea,
    StaticIndicatorLine,
    DynamicIndicatorArea,
    DynamicIndicatorLine,
}

[Serializable]
public class ObjectBankSet
{
    public ObjectCode name;
    public GameObject data;
}

[CreateAssetMenu(fileName = "ObjectBankSO", menuName = "Scriptable Objects/ObjectBankSO")]
public class ObjectBankSO : ScriptableObject
{
    public ObjectBankSet[] objectBankSet;
}
