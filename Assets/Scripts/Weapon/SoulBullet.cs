using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class SoulBullet : MonoBehaviour,IPoolingObject
{
    [SerializeField] private TrailRenderer tr;
    [SerializeField] private Rigidbody2D rb2D;
    [SerializeField] private WeaponDamager weaponDamager;
    [SerializeField] private float shootSpeed;
    [SerializeField] private float lastTime;
    
    public void OnBirth()
    {
        tr.Clear();
        weaponDamager.parent = PlayerStatus.Instance.gameObject;
        rb2D.linearVelocity = gameObject.transform.right*shootSpeed;
        StartCoroutine(LastTimeFlow());
    }

    public void OnDeathInit()
    {
        tr.Clear();
    }

    IEnumerator LastTimeFlow()
    {
        yield return new WaitForSeconds(lastTime);
        ObjectPoolManager.Instance.Return(gameObject);
    }
    
}
