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
        for (var i = 0; i < caseContext.Selections.Count; i++)
        {
            var selection = caseContext.Selections[i];
            selections.Add(BuildSelection(selection, playerContext));
        }

        return new HeroGameCaseDetailUIControlData(
            caseContext.CaseIndex,
            caseContext.StaticData,
            selections
        );
    }

    private static HeroGameCaseSelectionUIControlData BuildSelection(
        HeroGameCaseSelection selection,
        HeroPlayerContext playerContext)
    {
        return new HeroGameCaseSelectionUIControlData(
            playerContext.GetSuccessPercent(selection.StatRequirement),
            selection
        );
    }
}