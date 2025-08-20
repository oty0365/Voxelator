using System.Collections;
using UnityEngine;

[RequireComponent(typeof(ObjectTypeDefiner))]
public class ParticleObject : MonoBehaviour,IPoolingObject
{
    public ParticleSystem prt;
    private float waitTime;


    public virtual void OnDeathInit()
    {

    }
    public virtual void OnBirth()
    {
        waitTime = prt.main.duration;
        StartCoroutine(ParticleFlow());
    }
    private IEnumerator ParticleFlow()
    {
        prt.Play();
        yield return new WaitForSeconds(waitTime);
        ObjectPoolManager.Instance.Return(gameObject);
    }
}
