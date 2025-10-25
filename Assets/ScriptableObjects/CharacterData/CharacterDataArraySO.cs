using UnityEditor.U2D.Animation;
using UnityEngine;

[CreateAssetMenu(fileName = "CharacterDataArraySO", menuName = "Scriptable Objects/CharacterDataArraySO")]
public class CharacterDataArraySO : ScriptableObject
{
    public CharacterDataSO[] characterData;
}
