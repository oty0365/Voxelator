using UnityEngine;

[CreateAssetMenu(fileName = "AugmentData", menuName = "Scriptable Objects/AugmentData")]
public class AugmentData : ScriptableObject
{
    public string augmentName;
    public Sprite augmentSprite;
    [TextArea]
    public string augmentDescription;
    public GameObject augmentPrefab;
    
}
