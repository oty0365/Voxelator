using System.Collections;
using UnityEngine;

public class EffectComputeManager : SceneSingletonMonoBehaviour<EffectComputeManager>
{
    private IEnumerator FreezeFlow(GameObject target)
    {
        var statContainer = target.GetComponent<StatContainer>();
        statContainer.GetStat<LimitedStat>(StatusCode.Hp).SetBuff(BuffType.Add,-statContainer.GetStat<LimitedStat>(StatusCode.Hp).MaxValue*0.1f);
        var originSpeed = statContainer.GetStat<LimitedStat>(StatusCode.MoveSpeed).Value;
        statContainer.GetStat<UnlimitedStat>(StatusCode.MoveSpeed).SetBuff(BuffType.Add, -originSpeed);
        yield return new WaitForSeconds(1.2f);
        statContainer.GetStat<UnlimitedStat>(StatusCode.MoveSpeed).SetBuff(BuffType.Add, originSpeed);
    }
    private IEnumerator OverHeatFlow(GameObject target)
    {
        var statContainer = target.GetComponent<StatContainer>();
        for (int i = 0; i < 5; i++)
        {
            statContainer.GetStat<LimitedStat>(StatusCode.Hp).SetBuff(BuffType.Add,-statContainer.GetStat<LimitedStat>(StatusCode.Hp).MaxValue*0.04f);
            yield return new WaitForSeconds(1f);
        }
    }
    private IEnumerator VirusFlow(GameObject target)
    {
        var statContainer = target.GetComponent<StatContainer>();
        statContainer.GetStat<UnlimitedStat>(StatusCode.Atk).SetBuff(BuffType.Mul,-0.2f);
        for (int i = 0; i < 6; i++)
        {
            statContainer.GetStat<LimitedStat>(StatusCode.Hp).SetBuff(BuffType.Add,-statContainer.GetStat<LimitedStat>(StatusCode.Hp).MaxValue*0.02f);
            yield return new WaitForSeconds(1f);
        }
        statContainer.GetStat<UnlimitedStat>(StatusCode.Atk).SetBuff(BuffType.Mul,0.2f);
    }

    private IEnumerator DebugFlow(GameObject target)
    {
        var statContainer = target.GetComponent<StatContainer>();
        var originSpeed = statContainer.GetStat<LimitedStat>(StatusCode.MoveSpeed).Value;
        statContainer.GetStat<UnlimitedStat>(StatusCode.MoveSpeed).SetBuff(BuffType.Add, -originSpeed);
        yield return new WaitForSeconds(3f);
        statContainer.GetStat<UnlimitedStat>(StatusCode.MoveSpeed).SetBuff(BuffType.Add, originSpeed);
    }
}
