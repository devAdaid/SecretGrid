using System.Collections.Generic;
using UnityEngine;

public class StaticDataHolder
{
    private readonly List<HeroGameCaseStaticData> chapter1CaseList = new List<HeroGameCaseStaticData>();
    private readonly List<HeroGameCaseStaticData> chapter2CaseList = new List<HeroGameCaseStaticData>();

    private readonly Dictionary<int, List<HeroGameCaseStaticData>> casesByFixedDay = new Dictionary<int, List<HeroGameCaseStaticData>>();

    private bool isInitialized;

    public StaticDataHolder()
    {
        if (isInitialized) return;

        chapter1CaseList.Clear();
        chapter2CaseList.Clear();
        casesByFixedDay.Clear();

        var chapter1Cases = Resources.LoadAll<HeroGameCaseScriptableData>("Data/Case/Chapter1");
        foreach (var caseScriptableData in chapter1Cases)
        {
            var caseData = HeroGameCaseStaticData.Build(caseScriptableData);
            chapter1CaseList.Add(caseData);
        }

        var chapter2Cases = Resources.LoadAll<HeroGameCaseScriptableData>("Data/Case/Chapter2");
        foreach (var caseScriptableData in chapter2Cases)
        {
            var caseData = HeroGameCaseStaticData.Build(caseScriptableData);
            chapter2CaseList.Add(caseData);
        }

        var fixedDayCases = Resources.LoadAll<HeroGameCaseScriptableData>("Data/Case/Special");
        foreach (var caseScriptableData in fixedDayCases)
        {
            var caseData = HeroGameCaseStaticData.Build(caseScriptableData);
            casesByFixedDay.TryAdd(caseData.FixedDay, new List<HeroGameCaseStaticData>());
            casesByFixedDay[caseData.FixedDay].Add(caseData);
        }

        isInitialized = true;
    }

    public List<HeroGameCaseStaticData> GetChapter1CaseList()
    {
        return chapter1CaseList;
    }

    public List<HeroGameCaseStaticData> GetChapter2CaseList()
    {
        return chapter2CaseList;
    }

    public List<HeroGameCaseStaticData> GetFixedDayCaseList(int day)
    {
        if (casesByFixedDay.TryGetValue(day, out var caseDataList))
            return caseDataList;

        return new List<HeroGameCaseStaticData>();
    }
}
