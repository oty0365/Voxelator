using UnityEngine;

[CreateAssetMenu(fileName = "AugmentData", menuName = "Scriptable Objects/AugmentData")]
public class AugmentDataSO: ScriptableObject
{
    public string augmentName;
    public Sprite augmentSprite;
    public string augmentDescription;
    public GameObject augmentBehavior;
    public AugmentState augmentState;
    public int maxAugmentedCount;
}
