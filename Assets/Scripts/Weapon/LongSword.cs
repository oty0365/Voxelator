using UnityEngine;

public class LongSword : MonoBehaviour,IPoolingObject
{
    [SerializeField] private WeaponDamager ls;
    public void OnBirth()
    {
        ls.parent = PlayerStatus.Instance.gameObject;
    }
    public void OnDeathInit()
    {

    }

}
