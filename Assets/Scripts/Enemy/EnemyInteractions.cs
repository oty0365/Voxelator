using UnityEngine;

public class EnemyInteractions : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            EventManager.Instance.Invoke(ActionKey.OnPlayerHit, gameObject);
        }
    }
}
