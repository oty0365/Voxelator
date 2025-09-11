using System.Collections;
using UnityEngine;

public class AfterImageGenerator : MonoBehaviour,IPoolingObject
{
    [SerializeField] private GameObject afterImage;
    [SerializeField] private float alpha;
    private GameObject _currentObject;
    private Coroutine _currentSpawnFlow;
    private float _spawnTime;
    public void OnBirth()
    {
        
    }

    public void StartSpawn(GameObject obj, float time)
    {
        _currentObject = obj;
        _currentObject.GetComponent<ILifeSycler>().OnDeath += EndSpawn;
        _spawnTime = time;
        if (_currentSpawnFlow != null)
        {
            StopCoroutine(_currentSpawnFlow);
        }

        _currentSpawnFlow = StartCoroutine(SpawnFlow());
    }

    private IEnumerator SpawnFlow()
    {
        while (true)
        {
            var o = ObjectPoolManager.Instance.Get(afterImage,_currentObject.transform.position,new Vector3(_currentObject.transform.rotation.x,_currentObject.transform.rotation.y,_currentObject.transform.rotation.z));
            var s = _currentObject.GetComponent<SpriteRenderer>();
            o.GetComponent<AfterImage>().SetImage(s.sprite,alpha,_currentObject.transform.localScale,s.flipX,s.color);
            yield return new WaitForSeconds(_spawnTime);
        }

    }

    public void EndSpawn()
    {
        ObjectPoolManager.Instance.Return(gameObject);
    }
    
    public void OnDeathInit()
    {
        _currentObject.GetComponent<ILifeSycler>().OnDeath -= EndSpawn;
        StopCoroutine(_currentSpawnFlow);
        _currentSpawnFlow = null;
    }
}
