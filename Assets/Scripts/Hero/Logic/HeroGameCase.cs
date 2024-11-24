public class HeroGameCase
{
    public readonly int CaseIndex;
    public readonly HeroGameCaseStaticData StaticData;

    public HeroGameCase(int caseIndex, HeroGameCaseStaticData staticData)
    {
        CaseIndex = caseIndex;
        StaticData = staticData;
    }
}
