using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class JoyStickHandle : MonoBehaviour, IPointerDownHandler, IDragHandler, IPointerUpHandler
{
    public Action<Vector2> moveDirInputer;
    [SerializeField] float range;
    [SerializeField] bool isDraging;
    private Vector2 _originJoyTransform;

    private void Start()
    {
        _originJoyTransform = transform.position;
        moveDirInputer += PlayerInput.Instance.OnMove;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        isDraging = true;
    }

    public void OnDrag(PointerEventData eventData)
    {
        if(isDraging)
        {
            transform.position = eventData.pointerCurrentRaycast.worldPosition;
        }
        if (Vector2.Distance(_originJoyTransform, transform.position) > range)
        {
            OnPointerUp(null);
            return;
        }

        var dir = ((Vector2)transform.position - _originJoyTransform).normalized;
        moveDirInputer?.Invoke(dir);
       
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        moveDirInputer?.Invoke(Vector2.zero);
        transform.position = _originJoyTransform;
        isDraging = false;
    }
}
