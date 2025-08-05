using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class TouchScreenHandler : MonoBehaviour, IPointerDownHandler
{
    public Action<Vector2> dashDirInputer;
    public Action<Vector2> rotationInputer;

    private void Start()
    {
        dashDirInputer += PlayerInput.Instance.OnDash;
        rotationInputer += PlayerInput.Instance.OnRotate;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        Vector3 worldTouchPos = Camera.main.ScreenToWorldPoint(eventData.position);
        Vector2 playerPos = PlayerStatus.Instance.transform.position;
        Vector2 dashDir = ((Vector2)worldTouchPos - playerPos);
        dashDir.Normalize();

        dashDirInputer?.Invoke(dashDir);
        rotationInputer?.Invoke(dashDir);
    }
}
