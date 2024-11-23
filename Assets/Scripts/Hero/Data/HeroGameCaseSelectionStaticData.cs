public class HeroGameCaseSelectionStaticData
{
    public readonly string Description;
    public readonly HeroGameCaseStatRequirement StatRequirement;
    public readonly HeroGameCaseStatReward StatReward;

    public HeroGameCaseSelectionStaticData(string description, HeroGameCaseStatRequirement statRequirement, HeroGameCaseStatReward statReward)
    {
        Description = description;
        StatRequirement = statRequirement;
        StatReward = statReward;
    }
}
