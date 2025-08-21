using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerInteractions : MonoBehaviour,IEvent
{
    public bool isInfinite;
    [SerializeField] private Collider2D collider2D;
    [Header("플레이어 레이어 마스크")]
    [SerializeField] private LayerMask layerMask;
    [SerializeField] private LayerMask originMask;
    [Header("상호작용")]
    [SerializeField] private float interactionDistance;
    [SerializeField] private LayerMask interactionMask;
    private Collider2D[] _interatingObjs =  new Collider2D[0];
    private Dictionary<Collider2D,GameObject> _interactionDic = new();
    private Coroutine _currentInfiniteTimeFlow;

    private void Start()
    {
        StartCoroutine(InteractionFlow());
    }
    

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

    private IEnumerator InteractionFlow()
    {
        while (true)
        {
            var interactions =
                Physics2D.OverlapCircleAll(gameObject.transform.position, interactionDistance, interactionMask);
            var removed = _interatingObjs.Except(interactions);
            var added = interactions.Except(_interatingObjs);
            foreach (var r in removed)
            {
                ObjectPoolManager.Instance.Return(_interactionDic[r].gameObject);
                _interactionDic.Remove(r);
            }

            foreach (var a in added)
            {
                var button = ObjectPoolManager.Instance.Get(ObjectBankManager.Instance.Get("InteractionButton"),
                    a.transform.position, Vector3.zero);
                button.GetComponent<InteractionButton>().interacter = a.gameObject;
                if (!_interactionDic.ContainsKey(a))
                {
                    _interactionDic.Add(a, button);
                }
            }
            _interatingObjs = interactions.ToArray();
            yield return new WaitForSeconds(0.1f);
        }
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
