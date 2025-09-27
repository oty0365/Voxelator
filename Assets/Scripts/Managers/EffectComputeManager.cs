using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectComputeManager : SceneSingletonMonoBehaviour<EffectComputeManager>
{
    private delegate IEnumerator EffectFlow(GameObject target);
    private Dictionary<EffectType, EffectFlow> _effectFlows;

    private void Start()
    {
        _effectFlows = new Dictionary<EffectType, EffectFlow>
        {
            { EffectType.Freeze, FreezeFlow },
            { EffectType.OverHeat, OverHeatFlow },
            { EffectType.Virus, VirusFlow },
            { EffectType.Debug, DebugFlow }
        };
    }

    public void RunEffect(EffectType key, GameObject target)
    {
        var effectContainer = target.GetComponent<EffectContainer>();
        if (_effectFlows.TryGetValue(key, out var flow))
        {
            if (!effectContainer.HasEffect(key))
            {
                effectContainer.AddEffect(key);
                StartCoroutine(flow(target));
            }
        }
    }

    private IEnumerator FreezeFlow(GameObject target)
    {
        var statContainer = target.GetComponent<StatContainer>();
        statContainer.GetStat<LimitedStat>(StatusCode.Hp).AddBuff(BuffType.Add, -statContainer.GetStat<LimitedStat>(StatusCode.Hp).MaxValue * 0.1f);
        var originSpeed = statContainer.GetStat<LimitedStat>(StatusCode.MoveSpeed).Value;
        statContainer.GetStat<UnlimitedStat>(StatusCode.MoveSpeed).AddBuff(BuffType.Add, -originSpeed);

        yield return new WaitForSeconds(1.2f);

        statContainer.GetStat<UnlimitedStat>(StatusCode.MoveSpeed).AddBuff(BuffType.Add, originSpeed);
        target.GetComponent<EffectContainer>().RemoveEffect(EffectType.Freeze);
    }

    private IEnumerator OverHeatFlow(GameObject target)
    {
        var statContainer = target.GetComponent<StatContainer>();
        for (int i = 0; i < 5; i++)
        {
            statContainer.GetStat<LimitedStat>(StatusCode.Hp).AddBuff(BuffType.Add, -statContainer.GetStat<LimitedStat>(StatusCode.Hp).MaxValue * 0.04f);
            yield return new WaitForSeconds(1f);
        }
        target.GetComponent<EffectContainer>().RemoveEffect(EffectType.OverHeat);
    }

    private IEnumerator VirusFlow(GameObject target)
    {
        var statContainer = target.GetComponent<StatContainer>();
        statContainer.GetStat<UnlimitedStat>(StatusCode.Atk).AddBuff(BuffType.Mul, -0.2f);

        for (int i = 0; i < 6; i++)
        {
            statContainer.GetStat<LimitedStat>(StatusCode.Hp).AddBuff(BuffType.Add, -statContainer.GetStat<LimitedStat>(StatusCode.Hp).MaxValue * 0.02f);
            yield return new WaitForSeconds(1f);
        }
        statContainer.GetStat<UnlimitedStat>(StatusCode.Atk).AddBuff(BuffType.Mul, 0.2f);
        target.GetComponent<EffectContainer>().RemoveEffect(EffectType.Virus);
    }

    private IEnumerator DebugFlow(GameObject target)
    {
        var statContainer = target.GetComponent<StatContainer>();
        var originSpeed = statContainer.GetStat<LimitedStat>(StatusCode.MoveSpeed).Value;

        statContainer.GetStat<UnlimitedStat>(StatusCode.MoveSpeed).AddBuff(BuffType.Add, -originSpeed);
        yield return new WaitForSeconds(3f);
        statContainer.GetStat<UnlimitedStat>(StatusCode.MoveSpeed).AddBuff(BuffType.Add, originSpeed);
        target.GetComponent<EffectContainer>().RemoveEffect(EffectType.Debug);
    }
}

