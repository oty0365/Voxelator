using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class EffectIcon : MonoBehaviour,IPoolingObject
{
    public GameObject parent;
    [SerializeField] private SpriteRenderer sr;
    private MaterialPropertyBlock _metProps;
    private Coroutine _followParentFlow;
    public void OnBirth()
    {
        Initialize();
    }
    private void Initialize()
    {
        _metProps = new MaterialPropertyBlock();
        sr.GetPropertyBlock(_metProps);
        _metProps.SetFloat("_Fill", 0);
        sr.SetPropertyBlock(_metProps);
        _followParentFlow=StartCoroutine(FollowParentFlow());
    }
    public void SetProgressed(float value)
    {
        _metProps.SetFloat("_Fill", value);
        sr.SetPropertyBlock(_metProps);
    }

    private IEnumerator FollowParentFlow()
    {
        while (true)
        {
            if (parent != null)
            {
                gameObject.transform.position = parent.transform.position;
            }
            yield return null;
        }
    }
    public void OnDeathInit()
    {
        StopCoroutine(_followParentFlow);
    }
}
