using UnityEngine;

[CreateAssetMenu(fileName = "CharacterDataSO", menuName = "Scriptable Objects/CharacterDataSO")]
public class CharacterDataSO : ScriptableObject
{
    public string code;
    public string characterName;
    public Sprite characterSprite;
    public string description;
    public int coast;
    public PlayerBasicStatusDataSO playerBasicStatusDataSO;
}
