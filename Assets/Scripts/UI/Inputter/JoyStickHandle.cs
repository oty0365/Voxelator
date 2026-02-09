using System;
using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class JoyStickHandle : MonoBehaviour ,IPointerDownHandler,IPointerUpHandler
{
    public Action<Vector2> moveDirInputer;
    public Action<Vector2> rotationInputer;

    [SerializeField] private float range = 100f;
    [SerializeField] private float deadZone = 10f;
    [SerializeField] private float smoothSpeed = 10f; 
    [SerializeField] private RectTransform originJoyTransform;

    private RectTransform _joyTransform;
    private Image _originJoyImage;
    private Canvas _canvas;
    private Camera _uiCamera;

    private Vector2 _targetPosition;
    private Vector2 _smoothedPosition;
    private Coroutine _joystickFlow;

    private void Start()
    {
        _joyTransform = GetComponent<RectTransform>();
        _originJoyImage = originJoyTransform.GetComponent<Image>();
        _originJoyImage.enabled = false;
        
        moveDirInputer += PlayerInput.Instance.OnMove;
        rotationInputer += PlayerInput.Instance.OnFlip;
        
        _canvas = GetComponentInParent<Canvas>();
        if (_canvas.renderMode == RenderMode.ScreenSpaceCamera)
        {
            _uiCamera = _canvas.worldCamera;
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (_joystickFlow != null) StopCoroutine(_joystickFlow);
        _joystickFlow = StartCoroutine(JoystickProcessFlow());
    }

    private IEnumerator JoystickProcessFlow()
    {
        _originJoyImage.enabled = true;

        while (true)
        {
            Vector2 screenPos = Input.mousePosition; 
            Vector2 localPoint;

            RectTransformUtility.ScreenPointToLocalPointInRectangle(
                originJoyTransform,
                screenPos,
                _uiCamera,
                out localPoint
            );

            _targetPosition = Vector2.ClampMagnitude(localPoint, range);
            _smoothedPosition = Vector2.Lerp(_smoothedPosition, _targetPosition, Time.deltaTime * smoothSpeed);
            _joyTransform.localPosition = _smoothedPosition;

            if (_targetPosition.magnitude < deadZone)
            {
                moveDirInputer?.Invoke(Vector2.zero);
            }
            else
            {
                Vector2 dir = _targetPosition.normalized;
                moveDirInputer?.Invoke(dir);
                rotationInputer?.Invoke(dir);
            }

            yield return null;
        }
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (_joystickFlow != null)
        {
            StopCoroutine(_joystickFlow);
            _joystickFlow = null;
        }

        _originJoyImage.enabled = false;
        _targetPosition = Vector2.zero;
        _smoothedPosition = Vector2.zero;
        _joyTransform.localPosition = Vector2.zero;

        moveDirInputer?.Invoke(Vector2.zero);
    }

    private void OnDisable()
    {
        if (_joystickFlow != null)
        {
            StopCoroutine(_joystickFlow);
            _joystickFlow = null;
        }
    }
}