
using System;
using System.Collections.Generic;

public class HeroGameContext
{
    public readonly HeroPlayerContext Player;

    public List<HeroGameCase> CurrentCases;

    private List<HeroGameCaseStaticData> casePool;

    public HeroGameContext()
    {
        Player = new HeroPlayerContext(10, 10, 10);
        CurrentCases = new List<HeroGameCase>();

        casePool = new List<HeroGameCaseStaticData>();
        casePool.Add(TempBuildCaseStaticData1());
        casePool.Add(TempBuildCaseStaticData2());
        casePool.Add(TempBuildCaseStaticData3());

        PickCases();
    }

    public int GetSuccessPercent(int caseIndex, int selectIndex)
    {
        var caseData = CurrentCases[caseIndex];
        var selectData = caseData.StaticData.SelectionDataList[selectIndex];
        return Player.GetSuccessPercent(selectData.StatRequirement);
    }

    public void SelectAndProcess(int caseIndex, int selectIndex)
    {
        // TODO: 인덱스 검증
        var caseData = CurrentCases[caseIndex];
        var selectData = caseData.StaticData.SelectionDataList[selectIndex];
        var successPercent = Player.GetSuccessPercent(selectData.StatRequirement);

        var random = new Random();
        var randomValue = random.Next(100);
        if (randomValue < successPercent)
        {
            // 사건 성공
            Player.AddStatReward(selectData.StatReward);
        }
        else
        {
            // 사건 실패
        }

        // 다음 사건들을 구성한다.
        PickCases();
    }

    private void PickCases()
    {
        CurrentCases.Clear();

        // TODO: 랜덤으로 사건을 선택한다.
        casePool.Shuffle();

        for (var i = 0; i < 2; i++)
        {
            var heroCase = new HeroGameCase(i, casePool[i]);
            CurrentCases.Add(heroCase);
        }
    }

    private static HeroGameCaseStaticData TempBuildCaseStaticData1()
    {
        return new HeroGameCaseStaticData(
            "Case1", "The Elevator Entrapment", "People are stuck in an elevator in a high-rise building.",
            new List<HeroGameCaseSelectionStaticData>()
            {
                new HeroGameCaseSelectionStaticData(
                    "Communicate to keep them calm.",
                    new HeroGameCaseStatRequirement(0, 0, 20),
                    new HeroGameCaseStatReward(0, 0, 15)
                ),
                new HeroGameCaseSelectionStaticData(
                    "Send snacks via a makeshift pulley system.",
                    new HeroGameCaseStatRequirement(10, 5, 0),
                    new HeroGameCaseStatReward(5,5,0)
                ),
            }
        );
    }

    private static HeroGameCaseStaticData TempBuildCaseStaticData2()
    {
        return new HeroGameCaseStaticData(
            "Case2", "The Transportation Strike", "A citywide public transport strike immobilizes daily commuters.",
            new List<HeroGameCaseSelectionStaticData>()
            {
                new HeroGameCaseSelectionStaticData(
                    "Organize carpools and alternative transport.",
                    new HeroGameCaseStatRequirement(5, 5, 5),
                    new HeroGameCaseStatReward(3, 3, 3)
                ),
                new HeroGameCaseSelectionStaticData(
                    "Mediate negotiations to resolve the strike.",
                    new HeroGameCaseStatRequirement(0, 10, 0),
                    new HeroGameCaseStatReward(15, 0, 0)
                ),
            }
        );
    }

    private static HeroGameCaseStaticData TempBuildCaseStaticData3()
    {
        return new HeroGameCaseStaticData(
            "Case3", "The Flood Disaster", "Unprecedented rainfall leads to severe flooding, displacing thousands.",
            new List<HeroGameCaseSelectionStaticData>()
            {
                new HeroGameCaseSelectionStaticData(
                    "Assist in evacuation and rescue missions.",
                    new HeroGameCaseStatRequirement(10, 0, 0),
                    new HeroGameCaseStatReward(20, 0, 0)
                ),
                new HeroGameCaseSelectionStaticData(
                    "Coordinate donation drives for food and clothing.",
                    new HeroGameCaseStatRequirement(0, 0, 40),
                    new HeroGameCaseStatReward(0, 0, 100)
                ),
            }
        );
    }
}