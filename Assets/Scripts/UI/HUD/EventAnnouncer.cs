using System;
using System.Collections;
using TMPro;
using UnityEngine;

public class EventAnnouncer : MonoBehaviour,IEvent
{
    [Header("UI")]
    [SerializeField] private TextMeshProUGUI text;

    [Header("설정")]
    [SerializeField] private float inTime;
    [SerializeField] private float stayTime;
    [SerializeField] private float outTime;
    [SerializeField] private float fadeSpeed;

    private Coroutine _currentFadeFlow;
    private void Start()
    {
        text.gameObject.SetActive(false);
    }

    public void Show(string key)
    {
        if (_currentFadeFlow != null)
        {
            StopCoroutine(_currentFadeFlow);
        }
        _currentFadeFlow = StartCoroutine(FadeInAndOut());
        text.text = Scripter.Instance.Translation(key);
    }

    private IEnumerator FadeInAndOut()
    {
        text.gameObject.SetActive(true);
        text.color = Color.clear;
        for (float t = 0; t < inTime; t += Time.deltaTime)
        {
            text.color = Color.Lerp(text.color, Color.white, fadeSpeed * Time.deltaTime);
            yield return null;
        }
        text.color = Color.white;
        yield return new WaitForSeconds(stayTime);
        for (float t = 0; t < outTime; t += Time.deltaTime)
        {
            text.color = Color.Lerp(text.color, Color.clear, fadeSpeed * Time.deltaTime);
            yield return null;
        }
        text.color = Color.clear;
        text.gameObject.SetActive(false);
    }
    
    public void Subscribe()
    {
        EventManager.Instance.AddListener(EventKey.ShowEventBanner,new Action<string>(Show));
    }

    public void Unsubscribe()
    {
        EventManager.Instance.RemoveListener(EventKey.ShowEventBanner,new Action<string>(Show));
    }

    private void OnEnable()
    {
        Subscribe();
    }

    private void OnDisable()
    {
        Unsubscribe();
    }
    
}
