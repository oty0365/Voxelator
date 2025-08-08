using UnityEngine;
using UnityEngine.EventSystems;

public class CardUI : MonoBehaviour,IPointerDownHandler
{
    [SerializeField] int index;
    [SerializeField] AugmentUI augmentUI;
    public bool isSelected;
    public void OnPointerDown(PointerEventData eventData)
    {
        if (!isSelected)
        {
            augmentUI.AugmentSelection(index);
            isSelected = true;
        }
    }
}
