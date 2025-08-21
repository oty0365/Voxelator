using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;

public class InteractionButton : MonoBehaviour,IPoolingObject
{
    public GameObject interacter;
    private Coroutine _currentFallowInteracterFlow;
    public void OnInteract()
    {
        interacter.GetComponent<InteractionButton>()?.OnInteract();
    }

    public void OnBirth()
    {
        _currentFallowInteracterFlow = StartCoroutine(FallowInteracterFlow());
    }

    private IEnumerator FallowInteracterFlow()
    {
        while (true)
        {
            if (interacter != null)
            {
                gameObject.transform.position = interacter.transform.position;
            }
            yield return new WaitForEndOfFrame();
        }
    }
    public void OnDeathInit()
    {
        StopCoroutine(_currentFallowInteracterFlow);
    }
}
