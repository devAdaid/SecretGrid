using UnityEngine;

public class HeroGameContextHolder : MonoSingleton<HeroGameContextHolder>
{
    [SerializeField]
    private HeroGameUI ui;

    public HeroGameContext GameContext { get; private set; }

    private void Start()
    {
        ProcessGameStart();
    }

    public void RestartGame()
    {
        ProcessGameStart();
    }

    public bool SelectCaseSelection(int caseIndex, int selectionIndex)
    {
        if (GameContext.GameState != GameState.Playing)
        {
            Debug.LogError($"게임 상태가 Playing이 아닌데 선택 처리가 들어왔습니다. GameState[{GameContext.GameState}]"); ;
            return false;
        }

        var result = GameContext.SelectAndProcess(caseIndex, selectionIndex);
        ApplyStatUI();
        return result;
    }

    public void ProcessNext()
    {
        if (GameContext.GameState != GameState.Playing)
        {
            Debug.LogError($"게임 상태가 Playing이 아닌데 진행 처리가 들어왔습니다. GameState[{GameContext.GameState}]"); ;
            return;
        }

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

    private void ProcessGameStart()
    {
        GameContext = new HeroGameContext();
        GameContext.SetStart(Time.time);

        ui.HideScoreResultUI();
        ApplyStatUI();
        OnDayStarted(GameContext.Day);
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
        var result = GameContext.ProcessNext(Time.time);

        ApplyStatUI();

        switch (result)
        {
            case HeroGameProcessNextResult.NextDay:
                OnDayStarted(GameContext.Day);
                break;
            case HeroGameProcessNextResult.GameOverBySecretZero:
            case HeroGameProcessNextResult.GameEnd:
                LeaderboardSubmitScore.Instance.TotalScore = GameContext.GetScore();
                ui.ShowScoreResultUI(GameContext);
                break;
        }
    }
}
