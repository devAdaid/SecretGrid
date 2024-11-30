using System;

public partial class HeroGameContext
{
    public bool SelectAndProcess(int caseIndex, int selectIndex)
    {
        // TODO: 인덱스 검증
        var heroCase = CurrentCases[caseIndex];
        var selection = heroCase.Selections[selectIndex];
        var successPercent = Player.GetSuccessPercent(selection.StatRequirement);

        var result = false;
        var random = new Random();
        var randomValue = random.Next(100);
        if (randomValue < successPercent)
        {
            ProcessSelectionSuccess(selection);
            result = true;
        }
        else
        {
            ProcessSelectionFail(selection);
            result = false;
        }

        return result;
    }

    private void ProcessSelectionSuccess(HeroGameCaseSelection selection)
    {
        var heroCase = CurrentCases[selection.CaseIndex];
        if (specialCasePool.Contains(heroCase.StaticData))
        {
            specialCasePool.Remove(heroCase.StaticData);
        }

        Player.AddStatReward(selection.StatReward);
        AudioManager.I.PlaySFX(SFXType.Success);
    }

    private void ProcessSelectionFail(HeroGameCaseSelection selection)
    {
        Player.DecreaseSecret(selection.StaticData.DecreaseSecretValueOnFail);
        AudioManager.I.PlaySFX(SFXType.Fail);
    }
}