using UnityEngine;

public class HeroGameUI : MonoSingleton<HeroGameUI>, IHeroGameUI
{
    [SerializeField]
    private HeroGameCaseUI caseUI;

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
