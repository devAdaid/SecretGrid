using UnityEngine;

public class HeroGameContextHolder : MonoSingleton<HeroGameContextHolder>
{
    [SerializeField]
    private HeroGameUI ui;

    public HeroGameContext GameContext { get; private set; }

    private void Awake()
    {
        GameContext = new HeroGameContext();
    }

    private void Start()
    {
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
        ApplyStatUI();
    }

    private void ApplyStatUI()
    {
        ui.ApplyStatUI(GameContext.Player);
    }
}
