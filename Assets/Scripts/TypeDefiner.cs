using UnityEngine;

public enum EntityType
{
    Human,
    Bug,
    Monster,
    Machine
}

public class TypeDefiner : MonoBehaviour
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
}
