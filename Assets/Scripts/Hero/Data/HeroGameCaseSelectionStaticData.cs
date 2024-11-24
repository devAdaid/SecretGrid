public class HeroGameCaseSelectionStaticData
{
    public readonly string Description_Ko;
    public readonly string Description_En;
    public readonly HeroGameStatType MainRewardStatType;
    public readonly HeroGameStatType MainRequirementStatType;

    public HeroGameCaseSelectionStaticData(
        string description_ko,
        string description_en,
        HeroGameStatType mainRewardStatType,
        HeroGameStatType mainRequirementStatType)
    {
        Description_Ko = description_ko;
        Description_En = description_en;
        MainRewardStatType = mainRewardStatType;
        MainRequirementStatType = mainRequirementStatType;
    }

    public static HeroGameCaseSelectionStaticData Build(HeroGameCaseSelectionScriptableData data)
    {
        return new HeroGameCaseSelectionStaticData(
            data.Description_Ko,
            data.Description_En,
            data.MainRewardStatType,
            data.MainRequirementStatType
        );
    }
}
