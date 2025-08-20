using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class EnemyInteractions : MonoBehaviour
{
    [SerializeField] private AEnemy enemy;
    private readonly Dictionary<Collider2D, Coroutine> activeHits = new();

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            EventManager.Instance.Invoke(ActionKey.OnPlayerHit, gameObject);
        }

        if (other.CompareTag("Weapon"))
        {
            TryApplyDamage(other);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (activeHits.TryGetValue(other, out Coroutine routine))
        {
            StopCoroutine(routine);
            activeHits.Remove(other);
        }
    }

    private void TryApplyDamage(Collider2D weaponCollider)
    {
        if (activeHits.ContainsKey(weaponCollider))
        {
            return; 
        }
        var damagerObject = weaponCollider.GetComponent<Damager>();
        var damageData = weaponCollider.GetComponent<IDamager>().GetDamage(damagerObject.parent.GetComponent<IDamageStat>().GetStat());
        enemy.OnHit(damageData);
        if (gameObject.activeSelf)
        {
            Coroutine routine = StartCoroutine(HitCooldown(weaponCollider, damageData.time));
            activeHits[weaponCollider] = routine;
        }
    }

    private IEnumerator HitCooldown(Collider2D weaponCollider, float cooldown)
    {
        yield return new WaitForSeconds(cooldown);

        activeHits.Remove(weaponCollider);
        
        if (weaponCollider != null && weaponCollider.bounds.Intersects(GetComponent<Collider2D>().bounds))
        {
            TryApplyDamage(weaponCollider);
        }
    }
}