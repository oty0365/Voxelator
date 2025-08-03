using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[Flags]
public enum AugmentState
{
    None = 0,
    Stat = 1 << 0,
    Weapon = 1 << 1,
    Util = 1 << 2,
}

public class AugmentManager : HalfSingleMono<AugmentManager>
{
    public event Action<AugmentData[]> setUi;
    [SerializeField] AugmentDatas augmentDatas;
    private List<AugmentData> _augmentList = new();
    public int augmentedTime;

    private void Start()
    {
        _augmentList = new List<AugmentData>(augmentDatas.datas);
        StartCoroutine(CheckAugmentedTime());
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            AugmentSelection(AugmentState.Stat | AugmentState.Weapon | AugmentState.Util);
        }
    }

    public void AugmentSelection(AugmentState targetStates)
    {
        setUi?.Invoke(GetRandomAugments(targetStates));
        Time.timeScale = 0;
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

    public void RemoveData(AugmentData data)
    {
        _augmentList.Remove(data);
    }

    private AugmentData[] GetRandomAugments(AugmentState targetStates)
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

        return new AugmentData[] {
            filteredAugments[0],
            filteredAugments[1],
            filteredAugments[2],
            filteredAugments[3]
        };
    }
}