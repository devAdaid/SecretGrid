using System.Collections.Generic;

public partial class HeroGameContext
{
    private void ProcessNextDay()
    {
        // 다음 날로
        Day += 1;

        HandleDayStart();
    }
    
    private void HandleDayStart()
    {

        // TODO: 임시 챕터 2 이후 처리해둠. 추후 디벨롧.
        if (Day == 6)
        {
            AudioManager.I.PlayBGM(BGMType.Game2);

            normalCasePool.Clear();
            foreach (var caseData in CommonSingleton.I.StaticDataHolder.GetChapter2CaseList())
            {
                normalCasePool.Add(caseData);
            }
        }

        specialCasePool.Clear();

        // 다음 사건들을 구성한다.
        if (CommonSingleton.I.StaticDataHolder.TryGetDayData(Day, out var dayData))
        {
            MaxRemainPhase = dayData.MaxPhase;
            RemainPhase = dayData.MaxPhase;
            foreach (var caseIdData in dayData.CaseIdList)
            {
                var caseData = CommonSingleton.I.StaticDataHolder.GetCaseData(caseIdData.name);
                specialCasePool.Add(caseData);
                ProcessPickCases(PickSpecialCaseStaticDataList());
            }
        }
        else
        {
            MaxRemainPhase = null;
            RemainPhase = null;
            ProcessPickCases(PickNormalCaseStaticDataList());
        }
    }

    private void ProcessPickCases(List<HeroGameCaseStaticData> pickedCases)
    {
        CurrentCases.Clear();

        for (var caseIndex = 0; caseIndex < pickedCases.Count; caseIndex++)
        {
            var caseData = pickedCases[caseIndex];

            var selections = new List<HeroGameCaseSelection>();
            var selectionDataList = caseData.SelectionDataList;

            for (var selectionIndex = 0; selectionIndex < selectionDataList.Count; selectionIndex++)
            {
                var selectionData = selectionDataList[selectionIndex];
                var selection = BuildSelection(caseIndex, selectionIndex, selectionData);
                selections.Add(selection);
            }

            var heroCase = new HeroGameCase(caseIndex, caseData, selections);
            CurrentCases.Add(heroCase);
        }
    }

    private List<HeroGameCaseStaticData> PickNormalCaseStaticDataList()
    {
        var fixedDayCased = CommonSingleton.I.StaticDataHolder.GetFixedDayCaseList(Day);
        if (fixedDayCased.Count > 0)
        {
            return fixedDayCased;
        }

        var result = new List<HeroGameCaseStaticData>();
        var randomPickCount = HeroGameFormula.GetRandomCasePickCount(Day);
        normalCasePool.Shuffle();

        for (var caseIndex = 0; caseIndex < randomPickCount; caseIndex++)
        {
            result.Add(normalCasePool[caseIndex]);
        }

        return result;
    }
}