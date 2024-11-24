public interface IHeroGameUI
{
    void ApplyStatUI(HeroPlayerContext playerContext);

    void ApplyCaseUI(HeroGameCaseUIData data);
    void ActiveCaseListUI();
    void ActiveCaseDetailUI(int caseIndex);
}