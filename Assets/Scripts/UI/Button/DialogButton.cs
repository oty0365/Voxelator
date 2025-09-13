using UnityEngine;
using UnityEngine.EventSystems;

public class DialogButton : MonoBehaviour, IPointerDownHandler
{
    public void OnPointerDown(PointerEventData eventData)
    {
        DialogManager.Instance.NextText();
    }
}
