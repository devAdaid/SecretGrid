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

        if (GameContext.NeedProcessPhaseEnd())
        {
            OnPhaseEnd();
        }
        else
        {
            OnDayEnd(GameContext.Day);
        }
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
        if (GameContext.NeedProcessPhaseEnd())
        {
            ui.ApplyStatUI(GameContext.Day, GameContext.RemainPhase, GameContext.MaxRemainPhase, GameContext.Player);
        }
        else
        {
            ui.ApplyStatUI(GameContext.Day, null, null, GameContext.Player);
        }
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
            ui.DialogueUI.PlayDialogue(dayData.DayStartDialogue, dayData.IsDayStartDialogueSkippable, () => ProcessDayStart(day));
        }
        else
        {
            ProcessDayStart(day);
        }
    }

    private void ProcessDayStart(int day)
    {
        if (CommonSingleton.I.StaticDataHolder.TryGetDayData(day, out var dayData) && dayData.DayStartBgm != BGMType.Invalid)
        {
            AudioManager.I.PlayBGM(dayData.DayStartBgm);
        }
        ShowCaseListUI();
    }

    private void OnDayEnd(int day)
    {
        if (!GameContext.NeedProcessGameOver()
            && CommonSingleton.I.StaticDataHolder.TryGetDayData(day, out var dayData)
            && dayData.DayEndDialogue != null)
        {
            ui.DialogueUI.PlayDialogue(dayData.DayEndDialogue, dayData.IsDayEndDialogueSkippable, ProcessDayEnd);
        }
        else
        {
            ProcessDayEnd();
        }
    }

    private void OnPhaseEnd()
    {
        var result = GameContext.ProcessNext(Time.time);
        switch (result)
        {
            case HeroGameProcessNextResult.NextPhase:
                ShowCaseListUI();
                break;
            case HeroGameProcessNextResult.GameOverBySecretZero:
            case HeroGameProcessNextResult.GameOverByRemainPhaseZero:
            case HeroGameProcessNextResult.GameEnd:
                ProcessGameEnd(result);
                break;
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
            case HeroGameProcessNextResult.GameOverByRemainPhaseZero:
            case HeroGameProcessNextResult.GameEnd:
                ProcessGameEnd(result);
                break;
        }
    }

    private void ProcessGameEnd(HeroGameProcessNextResult result)
    {
        SecretGridServer.I.StartSendScore(GameContext);

        var dialogueName = GetDialogueName(result);
        if (CommonSingleton.I.StaticDataHolder.TryGetDialogue(dialogueName, out var dialogueData) && dialogueData != null)
        {
            ui.DialogueUI.PlayDialogue(dialogueData, true, OnGameEnd);
        }
        else
        {
            OnGameEnd();
        }
    }

    private void OnGameEnd()
    {
        ui.ShowScoreResultUI(GameContext);
    }

    private string GetDialogueName(HeroGameProcessNextResult result)
    {
        switch (result)
        {
            case HeroGameProcessNextResult.GameOverBySecretZero:
                return "GameOver_Secret";
            case HeroGameProcessNextResult.GameOverByRemainPhaseZero:
                if (CommonSingleton.I.StaticDataHolder.IsLastDay(GameContext.Day))
                {
                    return "GameOver_Phase_LastDay";
                }
                else
                {
                    return "GameOver_Phase";
                }
            case HeroGameProcessNextResult.GameEnd:
                if (GameContext.dialogueFlag.Contains("Side_K"))
                {
                    return "End2_DrK";
                }
                else
                {
                    return "End3_Xeros";
                }
        }
        return string.Empty;
    }
}
