public class HeroGameContextHolder : MonoSingleton<HeroGameContextHolder>
{
    public HeroGameContext GameContext { get; private set; }
    private IHeroGameUI ui;

    private void Awake()
    {
        GameContext = new HeroGameContext();

    }

    private void Start()
    {
        ui = HeroGameUI.I;
        ActiveCaseListUI();
    }

    public void SelectCaseSelection(int caseIndex, int selectionIndex)
    {
        GameContext.SelectAndProcess(caseIndex, selectionIndex);
        ActiveCaseListUI();
    }

    private void ActiveCaseListUI()
    {
        var caseData = HeroGameUIDataBuilder.BuildCase(GameContext);
        ui.ApplyCaseUI(caseData);
        ui.ActiveCaseListUI();
    }
}
