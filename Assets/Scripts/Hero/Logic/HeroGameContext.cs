using System.Collections.Generic;

public enum HeroGameProcessNextResult
{
    NextDay,
    NextPhase,
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

public partial class HeroGameContext
{
    public HeroPlayerContext Player { get; private set; }

    public List<HeroGameCase> CurrentCases;

    private List<HeroGameCaseStaticData> normalCasePool;
    private List<HeroGameCaseStaticData> specialCasePool;

    public int Day { get; private set; }
    public int? RemainPhase { get; private set; }

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

        normalCasePool = new List<HeroGameCaseStaticData>();
        foreach (var caseData in CommonSingleton.I.StaticDataHolder.GetChapter1CaseList())
        {
            normalCasePool.Add(caseData);
        }

        Day = 1;
        AudioManager.I.PlayBGM(BGMType.Game1);

        ProcessPickCases(PickNormalCaseStaticDataList());

        gameStartTime = 0f;
        GameState = GameState.Playing;
        Score = 0;
    }

    public void SetStart(float startTime)
    {
        gameStartTime = startTime;
    }

    public HeroGameProcessNextResult ProcessNext(float time)
    {
        if (Player.Secret <= 0)
        {
            ProcessGameOver(time);
            return HeroGameProcessNextResult.GameOverBySecretZero;
        }

        // TODO: 엔딩 기준은 데이터화
        if (Day == 25)
        {
            ProcessGameEnd(time);
            return HeroGameProcessNextResult.GameEnd;
        }

        // 페이즈가 남아있음 = 풀에서 케이스 구성
        if (RemainPhase > 0 || specialCasePool.Count > 0)
        {
            ProcessNextPhase();
            return HeroGameProcessNextResult.NextPhase;
        }

        // 다음 날로 진행
        ProcessNextDay();
        return HeroGameProcessNextResult.NextDay;
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
