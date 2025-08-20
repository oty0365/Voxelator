using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerInteractions : MonoBehaviour,IEvent
{
    public bool isInfinite;
    [SerializeField] private Collider2D collider2D;
    [Header("플레이어 레이어 마스크")]
    [SerializeField] private LayerMask layerMask;
    [SerializeField] private LayerMask originMask;
    
    private Coroutine _currentInfiniteTimeFlow;

    public void OnHit(GameObject damager)
    {
        var damagerObject = damager.GetComponent<Damager>();
        var damageData =damager.GetComponent<IDamager>().GetDamage(damagerObject.parent.GetComponent<IDamageStat>().GetStat());
        if (_currentInfiniteTimeFlow != null)
        {
            StopCoroutine(_currentInfiniteTimeFlow);
            isInfinite =  false;
            collider2D.excludeLayers = originMask;
        }
        _currentInfiniteTimeFlow = StartCoroutine(InfiniteTimeFlow(damageData.time));
    }
    
    private IEnumerator InfiniteTimeFlow(float time)
    {
        collider2D.excludeLayers = layerMask; 
        isInfinite = true; 
        yield return new WaitForSeconds(time); 
        isInfinite = false; 
        collider2D.excludeLayers = originMask;
    }
    
    public void Subscribe()
    {
        EventManager.Instance.AddListener(ActionKey.OnPlayerHit,new Action<GameObject>(OnHit));
    }

    public void Unsubscribe()
    {
        EventManager.Instance.RemoveListener(ActionKey.OnPlayerHit, new Action<GameObject>(OnHit));
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
