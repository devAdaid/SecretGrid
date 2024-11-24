using System.Collections.Generic;
using UnityEngine;

public class StaticDataHolder : PersistentSingleton<StaticDataHolder>
{
    private readonly List<HeroGameCaseStaticData> caseList = new();

    private bool isInitialized;

    protected override void Awake()
    {
        base.Awake();

        InitializeIfNot();
    }

    private void InitializeIfNot()
    {
        if (isInitialized) return;

        caseList.Clear();

        var cases = Resources.LoadAll<HeroGameCaseScriptableData>("Data/Case");
        foreach (var caseData in cases)
        {
            caseList.Add(HeroGameCaseStaticData.Build(caseData));
        }
    }

    public List<HeroGameCaseStaticData> GetCaseList()
    {
        InitializeIfNot();
        return caseList;
    }
}
