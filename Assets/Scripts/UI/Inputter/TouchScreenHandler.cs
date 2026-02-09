using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class TouchScreenHandler : MonoBehaviour, IPointerDownHandler,IPointerUpHandler
{
    public Action<Vector2> dashDirInputer;
    public Action<Vector2> rotationInputer;

    private void Start()
    {
        dashDirInputer += PlayerInput.Instance.OnDash;
        rotationInputer += PlayerInput.Instance.OnFlip;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        Vector3 worldTouchPos = Camera.main.ScreenToWorldPoint(eventData.position);
        var hit=Physics2D.Raycast(worldTouchPos, Vector2.zero, Mathf.Infinity);
        if (hit)
        {
            var interactbtn = hit.collider.gameObject.GetComponent<InteractionButton>();
            if (interactbtn != null)
            {
                interactbtn.OnInteract();
            }
        }
        EventManager.Instance.Invoke(EventKey.OnDisplayMouseDown,eventData);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        EventManager.Instance.Invoke(EventKey.OnDisplayMouseUp,eventData);
    }
    
}
