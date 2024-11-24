using System.Collections.Generic;

public static class HeroGameUIDataBuilder
{
    public static HeroGameCaseUIData BuildCase(HeroGameContext context)
    {
        var caseList = new List<HeroGameCaseDetailUIControlData>();
        foreach (var caseContext in context.CurrentCases)
        {
            var caseDetail = BuildCaseDetail(caseContext, context.Player);
            caseList.Add(caseDetail);
        }

        return new HeroGameCaseUIData(caseList);
    }

    private static HeroGameCaseDetailUIControlData BuildCaseDetail(HeroGameCase caseContext, HeroPlayerContext playerContext)
    {
        var selections = new List<HeroGameCaseSelectionUIControlData>();
        for (var i = 0; i < caseContext.StaticData.SelectionDataList.Count; i++)
        {
            var selectionData = caseContext.StaticData.SelectionDataList[i];
            selections.Add(BuildSelection(caseContext.CaseIndex, i, selectionData, playerContext));
        }

        return new HeroGameCaseDetailUIControlData(
            caseContext.CaseIndex,
            caseContext.StaticData,
            selections
        );
    }

    private static HeroGameCaseSelectionUIControlData BuildSelection(
        int caseIndex,
        int selectionIndex,
        HeroGameCaseSelectionStaticData selectionData,
        HeroPlayerContext playerContext)
    {
        return new HeroGameCaseSelectionUIControlData(
            caseIndex,
            selectionIndex,
            playerContext.GetSuccessPercent(selectionData.StatRequirement),
            selectionData
        );
    }
}