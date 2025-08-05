using System.Collections;
using Unity.Hierarchy;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerMove : MonoBehaviour
{
    [SerializeField] private PlayerController controller;
    public void SetDir(Vector2 dir)
    {
        if (controller.playerMoves == PlayerMoves.Dash)
        {
            return;
        }
        controller.currentDir = dir;
        if (dir == Vector2.zero)
        {
            controller.playerMoves = PlayerMoves.None;
            return;
        }
        controller.playerMoves = PlayerMoves.Walk;

    }
    public void SetRotaion(Vector2 dir)
    {
        var rotation = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, rotation);
    }
}
