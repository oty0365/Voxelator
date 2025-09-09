using System.Collections;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public abstract class AIndicator : MonoBehaviour,IPoolingObject
{
    [Header("요소")]
    public Image image;
    public RectTransform rectTransform;
    public TextMeshProUGUI mark;

    [Header("크기")]
    [SerializeField] protected float baseSize;
    public Vector2 size;
    public Vector2 localSize;
    
    [Header("색상")]
    public Color stareColor;
    public Color exeColor;

    [Header("애니메이션 속도")]
    public float fadeSpeed;
    public float fadeTime;
    
    protected Coroutine currentFadeInFlow;
    protected Coroutine currentFadeOutFlow;
    
    public void OnBirth()
    {
        OnStart();
    }

    public void SetBaseSize(float indiSize)
    {
        baseSize = indiSize;
        size.x = baseSize;
        size.y = baseSize;
        rectTransform.sizeDelta = size;
    }
    
    public virtual void OnStart()
    {
        size.x = baseSize;
        size.y = baseSize;
        rectTransform.sizeDelta = size;
        if (currentFadeInFlow != null)
        {
            StopCoroutine(currentFadeInFlow);
        }
        if (currentFadeOutFlow != null)
        {
            StopCoroutine(currentFadeOutFlow);
        }
        
        currentFadeInFlow = StartCoroutine(FadeInFlow());

    }
    
    public virtual void OnExecute()
    {
        image.color = exeColor;
    }

    public virtual void OnExit()
    {
        if (currentFadeOutFlow != null)
        {
            StopCoroutine(currentFadeOutFlow);
        }
        currentFadeOutFlow=StartCoroutine(FadeOutFlow());
    }

    private IEnumerator FadeInFlow()
    {
        mark.gameObject.SetActive(false);
        image.color = Color.clear;
        for (float t = 0; t < fadeTime; t += Time.deltaTime)
        {
            image.color = Color.Lerp(image.color, stareColor, fadeSpeed * Time.deltaTime);
            yield return null;
        }
        mark.gameObject.SetActive(true);
    }

    private IEnumerator FadeOutFlow()
    {
        mark.gameObject.SetActive(false);
        for (float t = 0; t < fadeTime; t += Time.deltaTime)
        {
            image.color = Color.Lerp(image.color, Color.clear,fadeSpeed * Time.deltaTime);
            yield return null;
        }
        ObjectPoolManager.Instance.Return(gameObject);
    }

    public void OnDeathInit()
    {
        StopCoroutine(currentFadeInFlow);
        StopCoroutine(currentFadeOutFlow);
        currentFadeInFlow = null;
        currentFadeOutFlow = null;
    }
}
