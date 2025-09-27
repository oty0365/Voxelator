using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;

public class InteractionButton : MonoBehaviour,IPoolingObject
{
    public GameObject interacter;
    private Transform _pos;
    private Coroutine _currentFallowInteracterFlow;
    public void OnInteract()
    {
        //Debug.Log(interacter);
        interacter.GetComponent<IInteractable>()?.OnInteract();
    }

    public void OnBirth()
    {
        _currentFallowInteracterFlow = StartCoroutine(FallowInteracterFlow());
    }

    public void SetPosition(Transform pos)
    {
        _pos = pos;
    }

    private IEnumerator FallowInteracterFlow()
    {
        while (true)
        {
            if (interacter != null)
            {
                gameObject.transform.position = _pos.position;
            }

            yield return null;
        }
    }
    public void OnDeathInit()
    {
        StopCoroutine(_currentFallowInteracterFlow);
    }
}
