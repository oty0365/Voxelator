using UnityEngine;

public class StaticAreaIndicator : AIndicator
{
    private Vector2 _targetPosition;
    private float _targetRotation;
    public void SetTarget(Vector2 pos, float rotation)
    {
        _targetPosition = pos;
        rectTransform.transform.position = _targetPosition;
        rectTransform.transform.rotation = Quaternion.Euler(0, 0, rotation);
    }
}
