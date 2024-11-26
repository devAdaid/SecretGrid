public interface IHeroGameUI
{
    void ApplyStatUI(int day, HeroPlayerContext playerContext);

    void ApplyCaseUI(HeroGameCaseUIData data);
    void ActiveCaseListUI();
    void ActiveCaseDetailUI(int caseIndex);

    void ShowResultUI(HeroGameCaseSelectionUIControlData data);
    void HideResultUI();
}