using UnityEngine;

public class DynamicAreaIndicator : AIndicator
{
    private Vector2 _targetPosition;
    private float _targetRotation;
    
    public void SetTarget(Vector2 pos, float rotation)
    {
        _targetRotation = rotation;
        _targetPosition = pos;
        rectTransform.transform.position = _targetPosition;
        rectTransform.transform.rotation = Quaternion.Euler(0, 0, _targetRotation);
    }

    private void Update()
    {
        rectTransform.transform.position = _targetPosition;
        rectTransform.transform.rotation = Quaternion.Euler(0, 0, _targetRotation);
    }
}
