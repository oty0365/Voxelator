
using System.Collections;
using UnityEngine;

public class SnakeBody : MonoBehaviour,IPoolingObject
{
    public float damage;
    [SerializeField] private SpriteRenderer sr;
    public GameObject target;
    public float speed;
    public float rotationSpeed;
    private Rigidbody2D rb2D;
    public static GameObject parent;
    void Start()
    {
        rb2D = GetComponent<Rigidbody2D>();
    }
    public void OnBirth()
    {
        sr.sprite = PlayerApperence.Instance.playerSkin.sprite;
        if (parent == null)
        {
            target = PlayerStatus.Instance.gameObject;
        }
        else
        {
            target = parent;
        }
        parent = gameObject;
    }
    public void OnDeathInit()
    {

    }
    private void FixedUpdate()
    {
        float distance = Vector2.Distance(target.transform.position, transform.position);

        float adjustedSpeed = Mathf.Lerp(0, speed, Mathf.Clamp01((distance - 0.55f) / 0.65f));

        Vector2 direction = (target.transform.position - transform.position).normalized;

        float targetAngle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        float angle = Mathf.LerpAngle(transform.eulerAngles.z, targetAngle, rotationSpeed * Time.fixedDeltaTime * 0.01f);
        transform.rotation = Quaternion.Euler(0, 0, angle);

        rb2D.linearVelocity = direction * adjustedSpeed;
    }
}

