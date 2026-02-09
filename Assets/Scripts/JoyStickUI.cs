using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class JoyStickUI : MonoBehaviour
{
    [SerializeField] private JoyStickHandle joyStickHandle;

    private void Start()
    {
        EventManager.Instance.AddListener(EventKey.OnDisplayMouseDown, new Action<PointerEventData>(MouseDown));
        EventManager.Instance.AddListener(EventKey.OnDisplayMouseUp, new Action<PointerEventData>(MouseUp));
    }

    private void MouseDown(PointerEventData eventData)
    {
        Vector3 worldTouchPos = Camera.main.ScreenToWorldPoint(eventData.position);
        transform.position = new Vector3(worldTouchPos.x, worldTouchPos.y, 0);
        gameObject.SetActive(true);
        joyStickHandle.OnPointerDown(eventData);
    }

    private void MouseUp(PointerEventData eventData)
    {
        Vector3 worldTouchPos = Camera.main.ScreenToWorldPoint(eventData.position);
        transform.position = new Vector3(worldTouchPos.x, worldTouchPos.y, 0);
        joyStickHandle.OnPointerUp(eventData);
        gameObject.SetActive(false);
    }
}