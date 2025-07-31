using System;
using System.Collections.Generic;
using UnityEngine;

public class AugmentManager : HalfSingleMono<AugmentManager>
{
    public event Action<AugmentData[]> setUi;
    [SerializeField] AugmentDatas augmentDatas;
    private List<AugmentData> _augmentList = new();

    public void Start()
    {
        _augmentList = new List<AugmentData>(augmentDatas.datas);
    }

    public void AugmentSelection()
    {
        setUi?.Invoke(GetRandomAugments());
        Time.timeScale = 0;
    }

    public void RemoveData(AugmentData data)
    {
        _augmentList.Remove(data);
    }
    
    private AugmentData[] GetRandomAugments()
    {
        if (augmentDatas.datas.Length < 4)
        {
            Debug.LogWarning("어그먼트 데이터가 4개 미만입니다.");
            return augmentDatas.datas;
        }

        List<AugmentData> dataList = _augmentList;
        
        for (var i = 0; i < 4; i++)
        {
            var randIndex = UnityEngine.Random.Range(i, dataList.Count);
            (dataList[i], dataList[randIndex]) = (dataList[randIndex], dataList[i]);
        }

        return new AugmentData[] { dataList[0], dataList[1], dataList[2] , dataList[3] };
    }
    
}
