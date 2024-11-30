using UnityEngine;

public class HeroGameUI : MonoSingleton<HeroGameUI>
{
    [SerializeField]
    private HeroGameStatUI statUI;

    [SerializeField]
    private HeroGameCaseUI caseUI;

    [SerializeField]
    private HeroGameCaseResultUI caseResultUI;

    [SerializeField]
    public HeroGameDialogueUI DialogueUI;

    [SerializeField]
    private HeroGameScoreResultUI scoreResultUI;

    [SerializeField]
    public HeroGameTooltipUI TooltipUI;

    public void ApplyStatUI(int day, HeroPlayerContext playerContext)
    {
        statUI.Apply(day, playerContext);
    }

    public void ApplyCaseUI(HeroGameCaseUIData data)
    {
        caseUI.Apply(data);
    }

    public void ActiveCaseListUI()
    {
        caseUI.ActiveCaseListUI();
    }

    public void ActiveCaseDetailUI(int caseIndex)
    {
        caseUI.ActiveDetailUI(caseIndex);
    }

    public void ShowCaseResultUI(HeroGameCaseSelectionUIControlData data)
    {
        caseResultUI.Show(data);
    }

    public void HideResultUI()
    {
        caseResultUI.Hide();
    }

    public void ShowScoreResultUI(HeroGameContext gameContext)
    {
        scoreResultUI.Show(gameContext);
    }

    public void HideScoreResultUI()
    {
        scoreResultUI.Hide();
    }
}
