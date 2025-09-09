using UnityEngine;

public class StaticLineIndicator : AIndicator
{
    private Vector2 _targetPosition;
    private GameObject _originPos;

    public void SetTarget(GameObject originPos, Vector2 targetPosition)
    {
        _originPos = originPos;
        _targetPosition = targetPosition;
    }
    
    private void Update()
    {
        var dir = _targetPosition-(Vector2)_originPos.transform.position;
        
        var angel = Mathf.Atan2(dir.y, dir.x)*Mathf.Rad2Deg;
        var midPoint = (Vector2)_originPos.transform.position + dir / 2;

        //var worldPoint = transform.localToWorldMatrix.MultiplyPoint3x4(halfVector);
        var distVal = Vector2.Distance(_originPos.transform.position, _targetPosition)*2;
        Debug.Log(_originPos.transform.position+","+distVal);
        var newSize = new Vector2(baseSize, baseSize*distVal);
        
        rectTransform.sizeDelta = newSize;
        rectTransform.position = midPoint;
        rectTransform.rotation = Quaternion.Euler(0, 0, angel+90);
    }
}
