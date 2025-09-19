using System.Collections;
using UnityEngine;

public class SoulBulletGenerator : MonoBehaviour,IPoolingObject
{
    [SerializeField] private WeaponDamager ls;
    [SerializeField] private AugmentedDatasSO augmentedDatas;
    [SerializeField] private GameObject bulletPrefab;

    private Coroutine _currentBulletFlow;
    public void OnBirth()
    {
        ls.parent = PlayerStatus.Instance.gameObject;
        if (_currentBulletFlow != null)
        {
            StopCoroutine(_currentBulletFlow);
        }
        _currentBulletFlow = StartCoroutine(SpawnBullet());
    }

    private IEnumerator SpawnBullet()
    {
        while (true)
        {
            yield return new WaitForSeconds(Extracter.Instance.ParseFloat(augmentedDatas.datas[1]));
            var dir = gameObject.transform.position - gameObject.transform.parent.transform.position;
            var angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
            var o=ObjectPoolManager.Instance.Get(bulletPrefab,gameObject.transform.position,new Vector3(0,0,angle));
            //o.GetComponent<WeaponDamager>().parent = 
        }
    }
    public void OnDeathInit()
    {
        StopCoroutine(_currentBulletFlow);
        _currentBulletFlow = null;
    }

}
