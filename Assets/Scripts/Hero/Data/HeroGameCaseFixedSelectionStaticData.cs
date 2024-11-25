public class HeroGameCaseFixedSelectionStaticData : IHeroGameCaseSelectionStaticData
{
    public string Description_Ko { get; private set; }
    public string Description_En { get; private set; }
    public int DecreaseSecretValueOnFail { get; private set; }
    public HeroGameCaseStatReward FixedStatReward { get; private set; }
    public HeroGameCaseStatRequirement FixedStatRequirement { get; private set; }

    public HeroGameCaseFixedSelectionStaticData(
        string description_ko,
        string description_en,
        int decreaseSecretValueOnFail,
        HeroGameCaseStatReward fixedStatReward,
        HeroGameCaseStatRequirement fixedStatRequirement)
    {
        Description_Ko = description_ko;
        Description_En = description_en;
        DecreaseSecretValueOnFail = decreaseSecretValueOnFail;
        FixedStatReward = fixedStatReward;
        FixedStatRequirement = fixedStatRequirement;
    }

    public static HeroGameCaseFixedSelectionStaticData Build(HeroGameCaseSelectionScriptableData data)
    {
        return new HeroGameCaseFixedSelectionStaticData(
            data.Description_Ko,
            data.Description_En,
            data.DecreaseSecretValueOnFail,
            data.FixedStatReward,
            data.FixedStatRequirement
        );
    }
}