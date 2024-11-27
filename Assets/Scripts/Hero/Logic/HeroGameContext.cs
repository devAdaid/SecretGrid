
using System;
using System.Collections.Generic;

public enum HeroGameProcessNextResult
{
    NextDay,
    GameEnd,
    GameOverBySecretZero,
}

public enum GameState
{
    NeedStart,
    Playing,
    EndByGameOver,
    EndByEnding
}

public class HeroGameContext
{
    public HeroPlayerContext Player { get; private set; }

    public List<HeroGameCase> CurrentCases;

    private List<HeroGameCaseStaticData> chapterCasePool;

    public int Day { get; private set; }

    public int Score { get; private set; }

    public GameState GameState { get; private set; }

    private float gameStartTime;
    private float gameEndTime;

    public HeroGameContext()
    {
        Initialize();
    }

    private void Initialize()
    {
        Player = new HeroPlayerContext(10, 10, 10);
        CurrentCases = new List<HeroGameCase>();

        chapterCasePool = new List<HeroGameCaseStaticData>();
        foreach (var caseData in CommonSingleton.I.StaticDataHolder.GetChapter1CaseList())
        {
            chapterCasePool.Add(caseData);
        }

        Day = 1;
        AudioManager.I.PlayBGM(BGMType.Game1);

        ProcessPickCases();

        gameStartTime = 0f;
        GameState = GameState.Playing;
        Score = 0;
    }

    public void SetStart(float startTime)
    {
        gameStartTime = startTime;
    }

    public int GetSuccessPercent(int caseIndex, int selectIndex)
    {
        var heroCase = CurrentCases[caseIndex];
        var selection = heroCase.Selections[selectIndex];
        return Player.GetSuccessPercent(selection.StatRequirement);
    }

    public bool SelectAndProcess(int caseIndex, int selectIndex)
    {
        // TODO: 인덱스 검증
        var heroCase = CurrentCases[caseIndex];
        var selection = heroCase.Selections[selectIndex];
        var successPercent = Player.GetSuccessPercent(selection.StatRequirement);

        var result = false;
        var random = new Random();
        var randomValue = random.Next(100);
        if (randomValue < successPercent)
        {
            ProcessSelectionSuccess(selection);
            result = true;
        }
        else
        {
            ProcessSelectionFail(selection);
            result = false;
        }

        return result;
    }

    public HeroGameProcessNextResult ProcessNext(float time)
    {
        if (Player.Secret <= 0)
        {
            ProcessGameOver(time);
            return HeroGameProcessNextResult.GameOverBySecretZero;
        }

        Day += 1;

        // TODO: 엔딩 기준은 데이터화
        if (Day == 26)
        {
            ProcessGameEnd(time);
            return HeroGameProcessNextResult.GameEnd;
        }

        // TODO: 임시 챕터 2 이후 처리해둠. 추후 디벨롧.
        if (Day == 6)
        {
            AudioManager.I.PlayBGM(BGMType.Game2);

            chapterCasePool.Clear();
            foreach (var caseData in CommonSingleton.I.StaticDataHolder.GetChapter2CaseList())
            {
                chapterCasePool.Add(caseData);
            }
        }


        // 다음 사건들을 구성한다.
        ProcessPickCases();
        return HeroGameProcessNextResult.NextDay;
    }

    private void ProcessPickCases()
    {
        CurrentCases.Clear();

        var pickedCases = PickCaseStaticDataList(out var isFixedDayCase);

        for (var caseIndex = 0; caseIndex < pickedCases.Count; caseIndex++)
        {
            var caseData = pickedCases[caseIndex];

            var selections = new List<HeroGameCaseSelection>();
            var selectionDataList = caseData.SelectionDataList;

            for (var selectionIndex = 0; selectionIndex < selectionDataList.Count; selectionIndex++)
            {
                var selectionData = selectionDataList[selectionIndex];
                var selection = BuildSelection(caseIndex, selectionIndex, selectionData);
                selections.Add(selection);
            }

            var heroCase = new HeroGameCase(caseIndex, caseData, selections);
            CurrentCases.Add(heroCase);
        }
    }

    private void ProcessSelectionSuccess(HeroGameCaseSelection selection)
    {
        Player.AddStatReward(selection.StatReward);
        AudioManager.I.PlaySFX(SFXType.Success);
    }

    private void ProcessSelectionFail(HeroGameCaseSelection selection)
    {
        Player.DecreaseSecret(selection.StaticData.DecreaseSecretValueOnFail);
        AudioManager.I.PlaySFX(SFXType.Fail);
    }

    private void ProcessGameOver(float endTime)
    {
        gameEndTime = endTime;
        GameState = GameState.EndByGameOver;

        //TODO: 초기화 대신 결과 UI 표시 및 랭킹 기록하도록 수정
        //Initialize();
    }

    private void ProcessGameEnd(float endTime)
    {
        gameEndTime = endTime;
        GameState = GameState.EndByEnding;

        //TODO: 초기화 대신 결과 UI 표시 및 랭킹 기록하도록 수정
        //Initialize();
    }

    public float GetPlayTime()
    {
        return Math.Max(gameEndTime - gameStartTime, 0);
    }

    public int GetScore()
    {
        var playTime = Math.Max(gameEndTime - gameStartTime, 0);
        return HeroGameFormula.CalculateScore(GameState == GameState.EndByEnding, Day, playTime, Player);
    }

    private List<HeroGameCaseStaticData> PickCaseStaticDataList(out bool isFixedDayCase)
    {
        var fixedDayCased = CommonSingleton.I.StaticDataHolder.GetFixedDayCaseList(Day);
        if (fixedDayCased.Count > 0)
        {
            isFixedDayCase = true;
            return fixedDayCased;
        }

        var result = new List<HeroGameCaseStaticData>();
        var randomPickCount = HeroGameFormula.GetRandomCasePickCount(Day);
        chapterCasePool.Shuffle();

        for (var caseIndex = 0; caseIndex < randomPickCount; caseIndex++)
        {
            result.Add(chapterCasePool[caseIndex]);
        }

        isFixedDayCase = false;
        return result;
    }

    private HeroGameCaseSelection BuildSelection(int caseIndex, int selectionIndex, IHeroGameCaseSelectionStaticData staticData)
    {
        HeroGameCaseStatReward statReward = new HeroGameCaseStatReward();
        HeroGameCaseStatRequirement statRequirement = new HeroGameCaseStatRequirement();

        switch (staticData)
        {
            case HeroGameCaseRandomSelectionStaticData data:
                {
                    var upVariationValue = selectionIndex;
                    var totalStatReward = HeroGameFormula.GetRandomStatRewardTotalCount(Day, upVariationValue);
                    statReward = HeroGameCaseStatReward.BuildRandom(totalStatReward, data.MainRewardStatType);

                    var totalStatRequirement = HeroGameFormula.GetRandomStatRequirementTotalCount(Day, upVariationValue);
                    statRequirement = HeroGameCaseStatRequirement.BuildRandom(totalStatRequirement, data.MainRequirementStatType);
                    break;
                }
            case HeroGameCaseFixedSelectionStaticData data:
                {
                    statReward = data.FixedStatReward;
                    statRequirement = data.FixedStatRequirement;
                    break;
                }
        }

        var selection = new HeroGameCaseSelection(caseIndex, selectionIndex, staticData, statReward, statRequirement);
        return selection;
    }
}
