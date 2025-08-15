using System;
using UnityEngine;

public class PlayerInput : SceneSingletonMonoBehaviour<PlayerInput>
{
    [SerializeField] private PlayerMove playerMove;
    [SerializeField] private PlayerDash playerDash;
    [SerializeField] private PlayerApperence apperence;
    public void OnMove(Vector2 dir)
    {
        playerMove.SetDir(dir);
    }
    public void OnFlip(Vector2 dir)
    {
        apperence.SetFlip(dir);
        //playerMove.SetRotaion(dir);
    }
    public void OnDash(Vector2 dir)
    {
        playerDash.Dash(dir);
    }
  
}
