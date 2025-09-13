using UnityEngine;

public class DynamicLineIndicator : AIndicator
{
    private GameObject _target;
    private GameObject _origin;

    public void SetTarget(GameObject origin, GameObject target)
    {
        _origin = origin;
        _target = target;
    }
    
    private void Update()
    {
        var dir = _target.transform.position - _origin.transform.position;
        var angel = Mathf.Atan2(dir.y, dir.x)*Mathf.Rad2Deg;
        var halfVector = dir / 2;
        var distVal = Vector2.Distance(_origin.transform.position, _target.transform.position)*2;
        var newSize = new Vector2(baseSize, baseSize*distVal);
        
        rectTransform.sizeDelta = newSize;
        rectTransform.position = halfVector;
        rectTransform.rotation = Quaternion.Euler(0, 0, angel);
    }
}
