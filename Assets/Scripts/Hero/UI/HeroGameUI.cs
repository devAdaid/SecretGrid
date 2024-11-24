using UnityEngine;

public class HeroGameUI : MonoSingleton<HeroGameUI>, IHeroGameUI
{
    [SerializeField]
    private HeroGameStatUI statUI;

    [SerializeField]
    private HeroGameCaseUI caseUI;

    public void ApplyStatUI(HeroPlayerContext playerContext)
    {
        statUI.Apply(playerContext);
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
}
