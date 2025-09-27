using UnityEngine;

public class ThroneyShield : MonoBehaviour,IPoolingObject
{
    [SerializeField] private WeaponDamager ts;
    [SerializeField] private float rotateSpeed;
    public void OnBirth()
    {
        ts.parent = PlayerStatus.Instance.gameObject;
    }

    private void Update()
    {
            var rot = transform.eulerAngles;
            rot.z += rotateSpeed * Time.deltaTime;
            transform.rotation = Quaternion.Euler(rot);
    }
    public void OnDeathInit()
    {

    }

}
