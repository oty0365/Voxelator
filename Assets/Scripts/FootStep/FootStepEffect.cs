using System.Collections;
using UnityEngine;

public class FootStepEffect : MonoBehaviour
{
    public GameObject footStepObj;
    public float coolDown;
    public Vector2 size;
    [SerializeField] private Transform footTransform;
    private Coroutine _currentSetpFlow;


    public void StartStep()
    {
        if(_currentSetpFlow == null)
        {
            _currentSetpFlow = StartCoroutine(StepFlow());
        }
        
    }
    private IEnumerator StepFlow()
    {
        while (true)
        {
            ObjectPoolManager.Instance.Get(footStepObj,footTransform.position, new Vector3(0, 0, 0),size);
            yield return new WaitForSeconds(coolDown);
        }
    }
    public void EndStep()
    {
        if (_currentSetpFlow != null)
        {
            StopCoroutine(_currentSetpFlow);
        }
        _currentSetpFlow = null;
    }

}
