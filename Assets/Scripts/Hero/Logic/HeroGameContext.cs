
using System;
using System.Collections.Generic;

public enum HeroGameProcessNextResult
{
    NextDay,
    GameEnd,
    GameOverBySecretZero,
}

public class HeroGameContext
{
    public HeroPlayerContext Player { get; private set; }

    public List<HeroGameCase> CurrentCases;

    private List<HeroGameCaseStaticData> chapterCasePool;

    // TODO: 챕터, 턴마다 난이도 밸런싱
    public int Day { get; private set; }

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

        Day = 0;
        AudioManager.I.PlayBGM(BGMType.Game1);

        ProcessPickCases();
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
            // 사건 성공
            Player.AddStatReward(selection.StatReward);
            AudioManager.I.PlaySFX(SFXType.Success);
            result = true;
        }
        else
        {
            // 사건 실패
            Player.DecreaseSecret(1);
            AudioManager.I.PlaySFX(SFXType.Fail);
            result = false;

            // 게임 오버
            if (Player.Secret == 0)
            {
                //Initialize();
            }
        }

        return result;
    }


    public HeroGameProcessNextResult ProcessNext()
    {
        if (Player.Secret == 0)
        {
            Initialize();
            return HeroGameProcessNextResult.GameOverBySecretZero;
        }

        Day += 1;

        // TODO: 게임 엔딩 처리
        if (Day == 25)
        {
            Initialize();
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

    private List<HeroGameCaseStaticData> PickCaseStaticDataList(out bool isFixedDayCase)
    {
        var fixedDayCased = CommonSingleton.I.StaticDataHolder.GetFixedDayCaseList(Day);
        if (fixedDayCased.Count > 0)
        {
            isFixedDayCase = true;
            return fixedDayCased;
        }

        var result = new List<HeroGameCaseStaticData>();
        var randomPickCount = GetRandomCasePickCount();
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
                    var totalStatReward = GetRandomStatRewardTotalCount(upVariationValue);
                    statReward = HeroGameCaseStatReward.BuildRandom(totalStatReward, data.MainRewardStatType);

                    var totalStatRequirement = GetRandomStatRequirementTotalCount(upVariationValue);
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

    private int GetRandomStatRewardTotalCount(int upVariationValue)
    {
        var random = new Random();

        // TODO: 데이터화
        if (Day < 5)
        {
            return random.Next(-3, 3) + 15 + upVariationValue * 3;
        }
        else if (Day < 10)
        {
            return random.Next(-5, 5) + 25 + upVariationValue * 5;
        }
        else if (Day < 17)
        {
            return random.Next(-5, 5) + 40 + upVariationValue * 8;
        }
        else
        {
            return random.Next(-10, 10) + 70 + upVariationValue * 10;
        }
    }

    private int GetRandomStatRequirementTotalCount(int upVariationValue)
    {
        var random = new Random();

        // TODO: 데이터화
        var countBase = GetRandomStatRequirementTotalCountBase();
        if (Day < 5)
        {
            return random.Next(-2, 2) + countBase + upVariationValue * 5;
        }
        else if (Day < 10)
        {
            return random.Next(-10, 10) + countBase + upVariationValue * 10;
        }
        else if (Day < 17)
        {
            return random.Next(-10, 10) + countBase + upVariationValue * 15;
        }
        else
        {
            return random.Next(-15, 15) + countBase + upVariationValue * 20;
        }
    }

    private int GetRandomStatRequirementTotalCountBase()
    {
        var result = 10;
        result += Day * 8;

        if (Day > 5)
        {
            result += 45;
            result += (Day - 5) * 10;
        }

        if (Day > 10)
        {
            result += 50;
            result += (Day - 10) * 15;
        }

        if (Day > 17)
        {
            result += 80;
            result += (Day - 17) * 30;
        }

        return result;
    }

    private int GetRandomCasePickCount()
    {
        // TODO: 데이터화
        if (Day < 10)
        {
            return 2;
        }
        return 3;
    }
}
