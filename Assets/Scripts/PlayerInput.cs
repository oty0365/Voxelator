using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInput : HalfSingleMono<PlayerInput>
{
    [SerializeField] private PlayerMove playerMove;
    private void Start()
    {
        
    }
    public void OnMove(Vector2 dir)
    {
        playerMove.SetDir(dir);
    }
    public void OnRotate(Vector2 dir)
    {
        playerMove.SetRotaion(dir);
    }
}
