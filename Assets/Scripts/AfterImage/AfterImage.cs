using System.Collections;
using UnityEngine;

public class AfterImage : MonoBehaviour,IPoolingObject
{
    public SpriteRenderer sr;
    public float fadeSpeed;
    public float lifeTime;
    private float _currentColor;

    public void OnBirth()
    {
        _currentColor = 1;
        sr.sprite = null;
        gameObject.transform.localScale = new Vector3(1, 1, 1);
        sr.color = new Color(_currentColor, _currentColor, _currentColor, 1);
    }

    public void OnDeathInit()
    {
        OnBirth();
    }

    public void SetImage(Sprite sprite, float alpha, Vector3 size,bool flipX,Color color)
    {
        sr.sprite = sprite;
        sr.flipX = flipX;
        sr.color = new Color(color.r,color.g, color.b, alpha);
        gameObject.transform.localScale = size;
        StartCoroutine(FadeFlow());

    }
    private IEnumerator FadeFlow()
    {
        for(var t = 0f; t <= lifeTime; t += Time.deltaTime)
        {
            sr.color = Color.Lerp(sr.color, Color.clear, fadeSpeed * Time.deltaTime);
            yield return null;
        }
        ObjectPoolManager.Instance.Return(gameObject);
    }
    /*private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("hitable"))
        {
            _currentColor = 1;
        }
    }*/
}
