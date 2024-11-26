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
        ShowCaseListUI();
    }

    public bool SelectCaseSelection(int caseIndex, int selectionIndex)
    {
        var result = GameContext.SelectAndProcess(caseIndex, selectionIndex);
        ApplyStatUI();
        return result;
    }

    public void ProcessNext()
    {
        GameContext.ProcessNext();
        ui.HideResultUI();

        ApplyStatUI();
        ShowCaseListUI();
    }

    private void ShowCaseListUI()
    {
        var caseData = HeroGameUIDataBuilder.BuildCase(GameContext);
        ui.ApplyCaseUI(caseData);
        ui.ActiveCaseListUI();
        ApplyStatUI();
    }

    private void ApplyStatUI()
    {
        ui.ApplyStatUI(GameContext.Day, GameContext.Player);
    }
}
