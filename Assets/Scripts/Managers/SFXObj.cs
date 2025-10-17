using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class SFXObj : MonoBehaviour, IPoolingObject
{
    public AudioSource currentSource;
    private Coroutine checkRoutine;
    
    public void OnBirth()
    {
        checkRoutine = StartCoroutine(CheckPlayingRoutine());
    }
    
    private IEnumerator CheckPlayingRoutine()
    {
        yield return new WaitUntil(() => currentSource != null && currentSource.clip != null);
        
        yield return new WaitWhile(() => currentSource.isPlaying);
        
        ObjectPoolManager.Instance.Return(gameObject);
    }
    
    public void OnDeathInit()
    {
        if (checkRoutine != null)
        {
            StopCoroutine(checkRoutine);
            checkRoutine = null;
        }
    }
}