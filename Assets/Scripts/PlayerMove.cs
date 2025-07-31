using UnityEngine;
[RequireComponent(typeof(Rigidbody2D))]
public class PlayerMove : MonoBehaviour
{
    private Rigidbody2D _rb2D;
    private Vector2 _dir;
    void Start()
    {
        _rb2D = GetComponent<Rigidbody2D>();
    } 

    public void SetDir(Vector2 dir)
    {
        _dir = dir;
    }
    public void SetRotaion(Vector2 dir)
    {
        var rotation = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, rotation);
    }

    private void FixedUpdate()
    {
        _rb2D.linearVelocity = _dir;
    }
}
