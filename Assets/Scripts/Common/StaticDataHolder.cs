using System.Collections.Generic;
using UnityEngine;

public class StaticDataHolder
{
    private readonly List<HeroGameCaseStaticData> chapter1CaseList = new List<HeroGameCaseStaticData>();
    private readonly List<HeroGameCaseStaticData> chapter2CaseList = new List<HeroGameCaseStaticData>();

    private readonly Dictionary<int, List<HeroGameCaseStaticData>> casesByFixedDay = new Dictionary<int, List<HeroGameCaseStaticData>>();
    private readonly Dictionary<string, HeroGameCaseStaticData> caseMap = new();
    private readonly Dictionary<int, HeroGameDayData> dayMap = new Dictionary<int, HeroGameDayData>();
    private readonly Dictionary<string, TextAsset> dialogueMap = new();

    public StaticDataHolder()
    {
        var chapter1Cases = Resources.LoadAll<HeroGameCaseScriptableData>("Data/Case/Chapter1");
        foreach (var caseScriptableData in chapter1Cases)
        {
            var caseData = HeroGameCaseStaticData.Build(caseScriptableData);
            chapter1CaseList.Add(caseData);
            caseMap[caseData.Id] = caseData;
        }

        var chapter2Cases = Resources.LoadAll<HeroGameCaseScriptableData>("Data/Case/Chapter2");
        foreach (var caseScriptableData in chapter2Cases)
        {
            var caseData = HeroGameCaseStaticData.Build(caseScriptableData);
            chapter2CaseList.Add(caseData);
            caseMap[caseData.Id] = caseData;
        }

        var fixedDayCases = Resources.LoadAll<HeroGameCaseScriptableData>("Data/Case/Special");
        foreach (var caseScriptableData in fixedDayCases)
        {
            var caseData = HeroGameCaseStaticData.Build(caseScriptableData);
            casesByFixedDay.TryAdd(caseData.FixedDay, new List<HeroGameCaseStaticData>());
            casesByFixedDay[caseData.FixedDay].Add(caseData);
            caseMap[caseData.Id] = caseData;
        }
        
        var dialogues = Resources.LoadAll<TextAsset>("Data/Dialogue");
        foreach (var dialogue in dialogues)
        {
            dialogueMap[dialogue.name] = dialogue;
        }

        var dayList = Resources.LoadAll<HeroGameDayData>("Data/Day");
        foreach (var dayData in dayList)
        {
            dayMap[dayData.Day] = dayData;
        }
    }

    public List<HeroGameCaseStaticData> GetChapter1CaseList()
    {
        return chapter1CaseList;
    }

    public List<HeroGameCaseStaticData> GetChapter2CaseList()
    {
        return chapter2CaseList;
    }

    public HeroGameCaseStaticData GetCaseData(string id)
    {
        return caseMap[id];
    }

    public List<HeroGameCaseStaticData> GetFixedDayCaseList(int day)
    {
        if (casesByFixedDay.TryGetValue(day, out var caseDataList))
            return caseDataList;

        return new List<HeroGameCaseStaticData>();
    }

    public bool TryGetDayData(int day, out HeroGameDayData data)
    {
        return dayMap.TryGetValue(day, out data);
    }
    
    public bool IsLastDay(int day)
    {
        if (!dayMap.TryGetValue(day, out var data))
        {
            return false;
        }

        return data.IsLastDay;
    }
    
    public bool TryGetDialogue(string name, out TextAsset dialogue)
    {
        return dialogueMap.TryGetValue(name, out dialogue);
    }
}
