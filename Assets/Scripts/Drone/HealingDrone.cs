using System.Collections;
using UnityEngine;

public class HealingDrone : MonoBehaviour,IPoolingObject
{
    [SerializeField] AugmentedDatasSO augmentedDatasSO;
    [SerializeField] private float height;
    [SerializeField] private float speed;
    [SerializeField] private Rigidbody2D rb2D;
    private Coroutine _currentDroneFlow;
    private Coroutine _currentHealFlow;
    private Vector2 _dir;
    public void OnBirth()
    {
       if(_currentDroneFlow != null)
       {
            StopCoroutine(_currentDroneFlow);
       }
       if (_currentHealFlow != null) 
       {
            StopCoroutine(_currentHealFlow);
       }
        _currentDroneFlow = StartCoroutine(DroneFlow());
        _currentHealFlow = StartCoroutine(HealFlow());
    }
    private void Update()
    {
        rb2D.linearVelocity = _dir*speed;
    }
    private IEnumerator DroneFlow()
    {
        while (true)
        {
            var playerTransform = PlayerStatus.Instance.gameObject.transform;
            _dir = new Vector2(playerTransform.position.x,playerTransform.position.y+height) - (Vector2)gameObject.transform.position ;
            yield return new WaitForSeconds(0.02f);
        }
    }
    private IEnumerator HealFlow()
    {
        while (true)
        {
            PlayerStatus.Instance.SetHp(PlayerStatus.Instance.playerHp.Value + Extracter.Instance.ParseFloat(augmentedDatasSO.datas[1]));
            yield return new WaitForSeconds(Extracter.Instance.ParseFloat(augmentedDatasSO.datas[0]));
        }

    }
    public void OnDeathInit()
    {

    }
}
