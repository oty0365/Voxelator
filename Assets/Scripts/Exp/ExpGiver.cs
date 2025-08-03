using System.Collections;
using UnityEngine;

public class ExpGiver : MonoBehaviour,IPoolingObject
{
    public float expAmount;
    [SerializeField] private TrailRenderer trail;
    public void OnBirth()
    {
        trail.Clear();
        StartCoroutine(GotoPlayerFlow());
    }

    private IEnumerator GotoPlayerFlow()
    {
        while (Vector3.Distance(PlayerStatus.Instance.gameObject.transform.position, transform.position) > 0.2f)
        {
            gameObject.transform.position = Vector2.MoveTowards(gameObject.transform.position, PlayerStatus.Instance.gameObject.transform.position, Time.deltaTime * 4f);
            yield return null;
        }
        PlayerStatus.Instance.SetExp(PlayerStatus.Instance.PlayerExp+expAmount);
        ObjectPooler.Instance.Return(gameObject);
    }

    public void OnDeathInit()
    {
        
    }
}
