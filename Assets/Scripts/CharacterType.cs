using UnityEngine;

public enum EntityType
{
    Human,
    Bug,
    Monster,
    Machine
}

public class CharacterType : MonoBehaviour
{
    [SerializeField] private EntityType entityType;

    public EntityType EntityType
    {
        get=>entityType;
        set
        {
            if (value != entityType)
            {
                entityType = value;
            }
        }
    }

    public Color GetColor()
    {
        var color = Color.white;
        switch (entityType)
        {
            case EntityType.Human:
                color = Color.white;
                break;
            case EntityType.Bug:
                color = new Color32(248, 74, 78,255);
                break;
            case EntityType.Monster:
                color = new Color32(74, 248, 122, 255);
                break;
            case EntityType.Machine:
                color = new Color32(74,177,248,255);
                break;
        }
        return color;
    }
}
