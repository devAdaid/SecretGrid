using System;
using System.Collections.Generic;

public partial class HeroGameContext
{
    private void ProcessNextPhase()
    {
        // 남은 페이즈 차감
        RemainPhase = Math.Max(0, RemainPhase.Value - 1);
        ProcessPickCases(PickSpecialCaseStaticDataList());
    }

    private List<HeroGameCaseStaticData> PickSpecialCaseStaticDataList()
    {
        var result = new List<HeroGameCaseStaticData>();
        var pickCount = Math.Min(3, specialCasePool.Count);

        for (var caseIndex = 0; caseIndex < pickCount; caseIndex++)
        {
            result.Add(specialCasePool[caseIndex]);
        }

        return result;
    }
}