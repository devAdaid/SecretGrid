public class HeroGameCaseSelection
{
    public readonly int CaseIndex;
    public readonly int SelectionIndex;
    public readonly IHeroGameCaseSelectionStaticData StaticData;
    public readonly HeroGameCaseStatReward StatReward;
    public readonly HeroGameCaseStatRequirement StatRequirement;

    public HeroGameCaseSelection(int caseIndex, int selectionIndex, IHeroGameCaseSelectionStaticData staticData, HeroGameCaseStatReward statReward, HeroGameCaseStatRequirement statRequirement)
    {
        CaseIndex = caseIndex;
        SelectionIndex = selectionIndex;
        StaticData = staticData;
        StatReward = statReward;
        StatRequirement = statRequirement;
    }

    public string GetTooltipText(int successProbability)
    {
        var result = string.Empty;
        if (StatRequirement != HeroGameCaseStatRequirement.None)
        {
            result += StatRequirement.ToCompareString(HeroGameContextHolder.I.GameContext.Player);

            if (successProbability < 100 && StaticData.DecreaseSecretValueOnFail > 0)
            {
                result += $"\nIf fail, {HeroGameStatType.Secret.ToIconString()}<color=#579C9A>-{StaticData.DecreaseSecretValueOnFail}</color>";
            }
        }
        return result;
    }
}