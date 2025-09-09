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
    public EntityType entityType;
    public Type currentType;
    public bool isElite;
    [SerializeField] private SpriteRenderer sr;
    
    public EntityType EntityType
    {
        get=>entityType;
        set
        {
            if (value != entityType)
            {
                entityType = value;
            }
            SetColor();
            SetType();
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

        if (isElite)
        {
            color =  Color.yellow;
        }
        return color;
    }
    public void SetColor()
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
        if (isElite)
        {
            color =  Color.yellow;
        }
        sr.color = color;
    }
    public void SetType()
    {
        switch (entityType)
        {
            case EntityType.Human:
                currentType = new Human();
                break;
            case EntityType.Bug:
                currentType = new Bug();
                break;
            case EntityType.Monster:
                currentType = new Monster();
                break;
            case EntityType.Machine:
                currentType = new Machine();
                break;
        }

        currentType.owner = gameObject;
    }
}
