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
        ApplyStatUI();
        OnDayStarted(GameContext.Day);
    }

    public bool SelectCaseSelection(int caseIndex, int selectionIndex)
    {
        var result = GameContext.SelectAndProcess(caseIndex, selectionIndex);
        ApplyStatUI();
        return result;
    }

    public void ProcessNext()
    {
        ui.HideResultUI();
        OnDayEnd(GameContext.Day);
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

    private void OnDayStarted(int day)
    {
        if (CommonSingleton.I.StaticDataHolder.TryGetDayData(day, out var dayData) && dayData.DayStartDialogue != null)
        {
            ui.DialogueUI.PlayDialogue(dayData.DayStartDialogue, ProcessDayStart);
        }
        else
        {
            ProcessDayStart();
        }
    }

    private void ProcessDayStart()
    {
        ShowCaseListUI();
    }

    private void OnDayEnd(int day)
    {
        if (CommonSingleton.I.StaticDataHolder.TryGetDayData(day, out var dayData) && dayData.DayEndDialogue != null)
        {
            ui.DialogueUI.PlayDialogue(dayData.DayEndDialogue, ProcessDayEnd);
        }
        else
        {
            ProcessDayEnd();
        }
    }

    private void ProcessDayEnd()
    {
        GameContext.ProcessNext();

        ApplyStatUI();
        ShowCaseListUI();
    }
}
