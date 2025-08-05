using UnityEngine;

public class SnakeGenerator : MonoBehaviour,IAugment,IPoolingObject
{
    [SerializeField] GameObject snakeBody;
    [TextableAugment, SerializeField] private float damage;
    public void Execute()
    {
        ObjectPooler.Instance.Get(snakeBody, PlayerStatus.Instance.gameObject.transform.position, new Vector3(0, 0, 0));
        ObjectPooler.Instance.Return(gameObject);
    }

    public void OnBirth()
    {
        snakeBody.GetComponent<SnakeBody>().damage = damage;
        Execute();

    }

    public void OnDeathInit()
    {

    }
}
