using System;
using System.Collections;
using UnityEngine;

public class WeaponCore : MonoBehaviour,IPoolingObject
{
    [SerializeField] private WeaponSetter weaponSetter;
    public WeaponCoreDatas weaponCoreDatas;
    private Coroutine _currentMoveFlow;
    public void OnBirth()
    {
        if (_currentMoveFlow == null)
        {
            _currentMoveFlow = StartCoroutine(MoveFlow());
        }
    }
    public void OnDeathInit()
    {
        if (_currentMoveFlow != null)
        {
            StopCoroutine(_currentMoveFlow);
        }
        _currentMoveFlow = null;
    }
    private IEnumerator MoveFlow()
    {
        while (true)
        {
            gameObject.transform.position = PlayerStatus.Instance.gameObject.transform.position;
            yield return new WaitForSeconds(0.01f);
        }
    }
    private void Update()
    {
        gameObject.transform.rotation = Quaternion.Euler(0, 0, gameObject.transform.rotation.eulerAngles.z + weaponCoreDatas.rotationSpeed * Time.deltaTime * PlayerStatus.Instance.playerAttackSpeed.Value);
    }

    public void Equip()
    {
        weaponSetter.SetWeapons();
    }
}
