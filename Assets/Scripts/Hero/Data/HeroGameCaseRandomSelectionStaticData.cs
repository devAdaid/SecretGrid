public interface IHeroGameCaseSelectionStaticData
{
    string Description_Ko { get; }
    string Description_En { get; }
    int DecreaseSecretValueOnFail { get; }
}

public class HeroGameCaseRandomSelectionStaticData : IHeroGameCaseSelectionStaticData
{
    public string Description_Ko { get; private set; }
    public string Description_En { get; private set; }
    public int DecreaseSecretValueOnFail { get; }
    public HeroGameStatType MainRewardStatType { get; private set; }
    //public HeroGameStatType MainRequirementStatType { get; private set; }

    public HeroGameCaseRandomSelectionStaticData(
        string description_ko,
        string description_en,
        int decreaseSecretValueOnFail,
        HeroGameStatType mainRewardStatType
        //HeroGameStatType mainRequirementStatType
        )
    {
        Description_Ko = description_ko;
        Description_En = description_en;
        DecreaseSecretValueOnFail = decreaseSecretValueOnFail;
        MainRewardStatType = mainRewardStatType;
        ///MainRequirementStatType = mainRequirementStatType;
    }

    public static HeroGameCaseRandomSelectionStaticData Build(HeroGameCaseSelectionScriptableData data)
    {
        return new HeroGameCaseRandomSelectionStaticData(
            data.Description_Ko,
            data.Description_En,
            data.DecreaseSecretValueOnFail,
            data.RandomMainRewardStatType
        //data.RandomMainRequirementStatType
        );
    }
}
