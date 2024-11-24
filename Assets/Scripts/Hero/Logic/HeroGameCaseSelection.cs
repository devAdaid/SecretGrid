public class HeroGameCaseSelection
{
    public readonly int CaseIndex;
    public readonly int SelectionIndex;
    public readonly HeroGameCaseSelectionStaticData StaticData;
    public readonly HeroGameCaseStatReward StatReward;
    public readonly HeroGameCaseStatRequirement StatRequirement;

    public HeroGameCaseSelection(int caseIndex, int selectionIndex, HeroGameCaseSelectionStaticData staticData, HeroGameCaseStatReward statReward, HeroGameCaseStatRequirement statRequirement)
    {
        CaseIndex = caseIndex;
        SelectionIndex = selectionIndex;
        StaticData = staticData;
        StatReward = statReward;
        StatRequirement = statRequirement;
    }
}