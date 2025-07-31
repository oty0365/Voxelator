using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class JoyStickHandle : MonoBehaviour, IPointerDownHandler, IDragHandler, IPointerUpHandler
{
    public Action<Vector2> moveDirInputer;
    public Action<Vector2> rotationInputer;
    [SerializeField] float range;
    [SerializeField] bool isDraging;
    [SerializeField] RectTransform originJoyTransform;
    private RectTransform _joyTransfrom;
    private Image _originJoyImage;
    private void Start()
    {
        _joyTransfrom = GetComponent<RectTransform>();
        _originJoyImage = originJoyTransform.gameObject.GetComponent<Image>();
        moveDirInputer += PlayerInput.Instance.OnMove;
        rotationInputer += PlayerInput.Instance.OnRotate;
        _originJoyImage.enabled = false;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        isDraging = true;
        _originJoyImage.enabled = true;
    }

    public void OnDrag(PointerEventData eventData)
    {
        if(isDraging)
        {
            _joyTransfrom.position = eventData.pointerCurrentRaycast.worldPosition;
        }
        if (Vector2.Distance(originJoyTransform.position, _joyTransfrom.position) > range)
        {
            OnPointerUp(null);
            return;
        }

        var dir = (_joyTransfrom.position - originJoyTransform.position).normalized;
        moveDirInputer?.Invoke(dir);
        rotationInputer?.Invoke(dir);
       
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        _originJoyImage.enabled = false;
        moveDirInputer?.Invoke(Vector2.zero);
        _joyTransfrom.position = originJoyTransform.position;
        isDraging = false;
    }
}
