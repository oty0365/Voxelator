using UnityEngine;

public class StaticLineIndicator : AIndicator
{
    private Vector2 _targetPosition;
    private GameObject _originPos;

    public void SetTarget(GameObject originPos, Vector2 targetPosition, Vector2 lsize)
    {
        _originPos = originPos;
        _targetPosition = targetPosition;
        localSize = lsize;
    }
    
    private void Update()
    {
        var dir = _targetPosition-(Vector2)_originPos.transform.position;
        var angel = Mathf.Atan2(dir.y, dir.x)*Mathf.Rad2Deg;
        var midPoint = (Vector2)_originPos.transform.position + dir / 2;
        var distVal = Vector2.Distance(_originPos.transform.position, _targetPosition)*1.35f;
        var newSize = new Vector2(baseSize, baseSize*distVal);
        
        rectTransform.localScale = localSize;
        rectTransform.sizeDelta = newSize;
        rectTransform.position = midPoint;
        rectTransform.rotation = Quaternion.Euler(0, 0, angel+90);
    }
}
