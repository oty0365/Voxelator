using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class TitleButton : MonoBehaviour,IPointerClickHandler,IButton
{
    public event Action onClick;
    public void OnPointerClick(PointerEventData eventData)
    {
        onClick?.Invoke();
    }
}
