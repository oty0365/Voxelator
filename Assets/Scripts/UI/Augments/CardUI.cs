using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class CardUI : MonoBehaviour,IPointerDownHandler
{
    [SerializeField] int index;
    public bool isSelected;
    public event Action<int> onSelected;
    
    public void OnPointerDown(PointerEventData eventData)
    {
        if (!isSelected)
        {
            onSelected?.Invoke(index);
            isSelected = true;
        }
    }
}
