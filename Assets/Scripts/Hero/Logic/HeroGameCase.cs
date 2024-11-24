using System.Collections.Generic;

public class HeroGameCase
{
    public readonly int CaseIndex;
    public readonly HeroGameCaseStaticData StaticData;
    public readonly List<HeroGameCaseSelection> Selections;

    public HeroGameCase(int caseIndex, HeroGameCaseStaticData staticData, List<HeroGameCaseSelection> selections)
    {
        CaseIndex = caseIndex;
        StaticData = staticData;
        Selections = selections;
    }
}
