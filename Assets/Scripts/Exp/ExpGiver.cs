using System.Collections;
using UnityEngine;

public class ExpGiver : MonoBehaviour,IPoolingObject
{
    public float expAmount;
    [SerializeField] private TrailRenderer trail;
    public static bool magnetToPlayer;
    private Coroutine magnetFlow;
   
    public void OnBirth()
    {
        trail.Clear();
        StartCoroutine(CheckToAbsorbFlow());
        if (magnetToPlayer)
        {
            StartCoroutine(GotoPlayerFlow());
        }
    }

    private IEnumerator GotoPlayerFlow()
    {

        while (Vector3.Distance(PlayerStatus.Instance.gameObject.transform.position, transform.position) > 0.32f)
        {
            gameObject.transform.position = Vector2.MoveTowards(gameObject.transform.position, PlayerStatus.Instance.gameObject.transform.position, Time.deltaTime * 4f);
            yield return null;
        }

    }
    private IEnumerator CheckToAbsorbFlow()
    {
        while (Vector3.Distance(PlayerStatus.Instance.gameObject.transform.position, transform.position) > 0.32f)
        {
            if (magnetToPlayer && magnetFlow == null)
            {
                magnetFlow = StartCoroutine(GotoPlayerFlow());
            }
            yield return new WaitForSeconds(0.05f);
        }
        PlayerStatus.Instance.SetExp(PlayerStatus.Instance.PlayerExp + expAmount);
        ObjectPooler.Instance.Return(gameObject);

    }

    public void OnDeathInit()
    {
        magnetFlow = null;
    }
}
