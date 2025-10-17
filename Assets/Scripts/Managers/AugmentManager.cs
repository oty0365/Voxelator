using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class AugmentStatus
{
    public int currentCount;
    public int maxCount;
}

[Flags]
public enum AugmentState
{
    None = 0,
    Stat = 1 << 0,
    Weapon = 1 << 1,
    Util = 1 << 2,
    Active = 1<<3,
}

public class AugmentManager : SceneSingletonMonoBehaviour<AugmentManager>
{

    [SerializeField] AugmentDatasSO augmentDatas;
    public event Action<AugmentDataSO[]> setUi;
    private List<AugmentDataSO> _augmentList = new();
    private Dictionary<AugmentDataSO, AugmentStatus> _augmentDict = new();
    public int augmentedTime;

    private void Start()
    {
        _augmentList = new List<AugmentDataSO>(augmentDatas.datas);
        StartCoroutine(CheckAugmentedTime());
        foreach(var i in _augmentList)
        {
            if (!_augmentDict.ContainsKey(i))
            {
                var a = new AugmentStatus();
                a.currentCount = 0;
                a.maxCount = i.maxAugmentedCount;
                _augmentDict.Add(i,a);
            }
        }
    }

    /*private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            AugmentSelection(AugmentState.Stat | AugmentState.Weapon | AugmentState.Util);
        }
    }*/
    public int GetAugmentedCount(AugmentDataSO key)
    {
        if (_augmentDict.ContainsKey(key))
        {
            return _augmentDict[key].currentCount;
        }
        return 0;
    }

    public void AugmentSelection(AugmentState targetStates)
    {
        setUi?.Invoke(GetRandomAugments(targetStates));
        EventManager.Instance?.Invoke(EventKey.StopShake);
        EventManager.Instance?.Invoke(EventKey.SetTimeScale,0);
    }

    public void ConsumedAugment(AugmentDataSO key)
    {
        if (_augmentDict.ContainsKey(key))
        {
            if (_augmentDict[key].currentCount + 1 >= _augmentDict[key].maxCount)
            {
                if (_augmentList.Contains(key))
                {
                    _augmentList.Remove(key);
                }
                else
                {
                    return;
                }
            }
            _augmentDict[key].currentCount++;
        }
    }

    private IEnumerator CheckAugmentedTime()
    {
        while (true)
        {
            if (PlayerStatus.Instance.PlayerLevel - augmentedTime > 1)
            {
                AugmentSelection(AugmentState.Stat | AugmentState.Weapon | AugmentState.Util);
                augmentedTime++;
            }
            yield return new WaitForSeconds(0.02f);
        }
    }

    public void RemoveData(AugmentDataSO data)
    {
        _augmentList.Remove(data);
    }

    private AugmentDataSO[] GetRandomAugments(AugmentState targetStates)
    {
        var filteredAugments = _augmentList.Where(augment =>(augment.augmentState & targetStates) != 0).ToList();

        if (filteredAugments.Count < 4)
        {
            Debug.LogWarning($"필터링된 어그먼트 데이터가 4개 미만입니다. (현재 {filteredAugments.Count}개)");
            return filteredAugments.Take(Math.Min(4, filteredAugments.Count)).ToArray();
        }
        for (var i = 0; i < 4; i++)
        {
            var randIndex = UnityEngine.Random.Range(i, filteredAugments.Count);
            (filteredAugments[i], filteredAugments[randIndex]) = (filteredAugments[randIndex], filteredAugments[i]);
        }

        return new AugmentDataSO[] {
            filteredAugments[0],
            filteredAugments[1],
            filteredAugments[2],
            filteredAugments[3]
        };
    }
}