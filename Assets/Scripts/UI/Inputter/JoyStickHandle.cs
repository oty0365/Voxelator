using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class JoyStickHandle : MonoBehaviour, IPointerDownHandler, IDragHandler, IPointerUpHandler
{
    public Action<Vector2> moveDirInputer;
    public Action<Vector2> rotationInputer;

    [SerializeField] private float range = 100f;
    [SerializeField] private float deadZone = 10f;
    [SerializeField] private float smoothThreshold = 5f; 
    [SerializeField] private float smoothSpeed = 10f; 
    [SerializeField] private RectTransform originJoyTransform;
    [SerializeField] private bool isDraging;

    private RectTransform _joyTransform;
    private Image _originJoyImage;
    private Canvas _canvas;
    private Camera _uiCamera;

    private Vector2 _targetPosition;
    private Vector2 _smoothedPosition;
    private Vector2 _lastValidInput;

    private void Start()
    {
        _joyTransform = GetComponent<RectTransform>();
        _originJoyImage = originJoyTransform.GetComponent<Image>();
        _originJoyImage.enabled = false;
        moveDirInputer += PlayerInput.Instance.OnMove;
        rotationInputer += PlayerInput.Instance.OnRotate;
        _canvas = GetComponentInParent<Canvas>();
        if (_canvas.renderMode == RenderMode.ScreenSpaceCamera)
        {
            _uiCamera = _canvas.worldCamera;
        }

        _targetPosition = Vector2.zero;
        _smoothedPosition = Vector2.zero;
        _lastValidInput = Vector2.zero;
    }

    private void Update()
    {
        if (isDraging)
        {
            _smoothedPosition = Vector2.Lerp(_smoothedPosition, _targetPosition, Time.deltaTime * smoothSpeed);
            _joyTransform.localPosition = _smoothedPosition;
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        isDraging = true;
        _originJoyImage.enabled = true;

        Vector2 screenPos = eventData.position;
        Vector2 localPoint;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            originJoyTransform,
            screenPos,
            _uiCamera,
            out localPoint
        );

        localPoint = Vector2.ClampMagnitude(localPoint, range);
        _targetPosition = localPoint;
        _smoothedPosition = localPoint;
        _joyTransform.localPosition = localPoint;
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (!isDraging) return;

        Vector2 screenPos = eventData.position;
        Vector2 localPoint;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            originJoyTransform,
            screenPos,
            _uiCamera,
            out localPoint
        );

        localPoint = Vector2.ClampMagnitude(localPoint, range);

        float distanceFromLast = Vector2.Distance(localPoint, _targetPosition);
        if (distanceFromLast > smoothThreshold)
        {
            _targetPosition = localPoint;
        }


        Vector2 inputVector = _targetPosition;

        if (inputVector.magnitude < deadZone)
        {
            moveDirInputer?.Invoke(Vector2.zero);
            _lastValidInput = Vector2.zero;
            return;
        }

        Vector2 dir = inputVector.normalized;

        if (_lastValidInput != Vector2.zero)
        {
            float angleDifference = Vector2.Angle(_lastValidInput, dir);
            if (angleDifference < 5f) 
            {
                dir = _lastValidInput;
            }
        }

        _lastValidInput = dir;
        moveDirInputer?.Invoke(dir);
        rotationInputer?.Invoke(dir);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        isDraging = false;
        _originJoyImage.enabled = false;

        _targetPosition = Vector2.zero;
        _smoothedPosition = Vector2.zero;
        _joyTransform.localPosition = Vector2.zero;

        moveDirInputer?.Invoke(Vector2.zero);
        _lastValidInput = Vector2.zero;
    }
}