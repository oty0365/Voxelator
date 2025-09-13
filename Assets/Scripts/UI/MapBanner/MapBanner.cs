using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MapBanner : MonoBehaviour, IEvent
{
    [Header("UI")]
    [SerializeField] private GameObject banner;
    [SerializeField] private Image mapBannerImage;
    [SerializeField] private TextMeshProUGUI mapBannerText;
    
    [Header("Speed")]
    [SerializeField] private float fadeInTime=2f;
    [SerializeField] private float stayTime=1f;
    [SerializeField] private float fadeOutTime=2f;
    [SerializeField] private float fadeSpeed =10f;
    
    private Coroutine _currentMapFadeFlow;

    public void FadeInOutMapBanner()
    {
        if (_currentMapFadeFlow != null)
        {
            banner.SetActive(false);
            StopCoroutine(_currentMapFadeFlow);
            _currentMapFadeFlow = null;
        }
        _currentMapFadeFlow = StartCoroutine(MapFadeFlow());
    }
    
    private IEnumerator MapFadeFlow()
    {
        var color = Color.clear;
        mapBannerImage.color = color;
        mapBannerText.color = color;
        banner.SetActive(true);
        for (var i = 0f; i < fadeInTime; i += Time.deltaTime)
        {
            color=Color.Lerp(color, Color.white, fadeSpeed * Time.deltaTime);
            mapBannerImage.color = color;
            mapBannerText.color = color;
            yield return null;
        }
        yield return new WaitForSeconds(stayTime);
        for (var i = 0f; i < fadeOutTime; i += Time.deltaTime)
        {
            color=Color.Lerp(color, Color.clear, fadeSpeed * Time.deltaTime);
            mapBannerImage.color = color;
            mapBannerText.color = color;
            yield return null;
        }
        color = Color.clear;
        mapBannerImage.color = color;
        mapBannerText.color = color;
        banner.SetActive(false);
    }
    public void Subscribe()
    {
        EventManager.Instance.AddListener(EventKey.ShowMapBanner,new Action(FadeInOutMapBanner));
    }

    public void Unsubscribe()
    {
        EventManager.Instance.RemoveListener(EventKey.ShowMapBanner,new Action(FadeInOutMapBanner));
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
