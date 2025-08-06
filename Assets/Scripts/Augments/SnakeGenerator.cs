using UnityEngine;

public class SnakeGenerator : AAugment,IPoolingObject
{
    [SerializeField] GameObject snakeBody;
    public override void Execute()
    {
        ObjectPooler.Instance.Get(snakeBody, PlayerStatus.Instance.gameObject.transform.position, new Vector3(0, 0, 0));
        UpLoad();
        ObjectPooler.Instance.Return(gameObject);
    }

    public void OnBirth()
    {
        Execute();
    }

    public void OnDeathInit()
    {

    }
}
