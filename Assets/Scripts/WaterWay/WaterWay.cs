using System.Collections;
using UnityEngine;

public class WaterWay : MonoBehaviour,IPoolingObject
{
    [SerializeField] AnimationClip clip;
    [SerializeField] Animator ani;
    private string _playName = "Waterway";
    private Coroutine _waterwayCoroutine;
    public void OnBirth()
    {
        if(_waterwayCoroutine == null)
        {
            _waterwayCoroutine = StartCoroutine(WaterwayFlow());
        }
    }
    private IEnumerator WaterwayFlow()
    {
        ani.Play(_playName);
        yield return new WaitForSeconds(clip.length);
        ObjectPoolManager.Instance.Return(gameObject);
    }

    public void OnDeathInit()
    {
        _waterwayCoroutine = null;
    }
}
