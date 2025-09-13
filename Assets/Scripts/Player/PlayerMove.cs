using System;
using System.Collections;
using Unity.Hierarchy;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerMove : MonoBehaviour,IEvent
{
    [SerializeField] private FootStepEffect stepEffect;
    private event Action onFootStepStart;
    private event Action onFootStepEnd;
    private PlayerController controller;

    public void Start()
    {
        controller = PlayerController.Instance;
    }

    public void SetDir(Vector2 dir)
    {
        if (controller.playerMoves == EntityMoves.Dash)
        {
            return;
        }
        controller.currentDir = dir;
        if (dir == Vector2.zero)
        {
            controller.playerMoves = EntityMoves.Idle;
            onFootStepEnd?.Invoke();
            return;
        }
        controller.playerMoves = EntityMoves.Walk;
        onFootStepStart?.Invoke();

    }
    public void SetRotaion(Vector2 dir)
    {
        var rotation = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, rotation);
    }
    public void Subscribe()
    {
        onFootStepStart += stepEffect.StartStep;
        onFootStepEnd += stepEffect.EndStep;
    }
    public void Unsubscribe()
    {
        onFootStepStart -= stepEffect.StartStep;
        onFootStepEnd -= stepEffect.EndStep;
    }
    private void OnEnable()
    {
        Subscribe();
    }
    private void OnDisable()
    {
        Unsubscribe();
    }
}
