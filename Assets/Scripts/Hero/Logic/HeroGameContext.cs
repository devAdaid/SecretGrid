
using System;
using System.Collections.Generic;

public class HeroGameContext
{
    public readonly HeroPlayerContext Player;

    public List<HeroGameCase> CurrentCases;

    private List<HeroGameCaseStaticData> casePool;

    // TODO: 챕터, 턴마다 난이도 밸런싱
    private int turn;

    public HeroGameContext()
    {
        Player = new HeroPlayerContext(10, 10, 10);
        CurrentCases = new List<HeroGameCase>();

        casePool = new List<HeroGameCaseStaticData>();
        foreach (var caseData in StaticDataHolder.I.GetCaseList())
        {
            casePool.Add(caseData);
        }

        turn = 0;

        PickCases();
    }

    public int GetSuccessPercent(int caseIndex, int selectIndex)
    {
        var heroCase = CurrentCases[caseIndex];
        var selection = heroCase.Selections[selectIndex];
        return Player.GetSuccessPercent(selection.StatRequirement);
    }

    public void SelectAndProcess(int caseIndex, int selectIndex)
    {
        // TODO: 인덱스 검증
        var heroCase = CurrentCases[caseIndex];
        var selection = heroCase.Selections[selectIndex];
        var successPercent = Player.GetSuccessPercent(selection.StatRequirement);

        var random = new Random();
        var randomValue = random.Next(100);
        if (randomValue < successPercent)
        {
            // 사건 성공
            Player.AddStatReward(selection.StatReward);
        }
        else
        {
            // 사건 실패
        }

        turn += 1;

        //TODO: 게임오버, 챕터 처리
        // 다음 사건들을 구성한다.
        PickCases();
    }

    private void PickCases()
    {
        CurrentCases.Clear();

        casePool.Shuffle();

        var random = new Random();
        for (var caseIndex = 0; caseIndex < 2; caseIndex++)
        {
            var caseData = casePool[caseIndex];

            var selections = new List<HeroGameCaseSelection>();
            for (var selectionIndex = 0; selectionIndex < caseData.SelectionDataList.Count; selectionIndex++)
            {
                var selectionData = caseData.SelectionDataList[selectionIndex];

                //TODO: 턴에 따라 보상, 조건 밸런싱
                var totalStatReward = random.Next(10, 15);
                var statReward = HeroGameCaseStatReward.BuildRandom(totalStatReward, selectionData.MainRewardStatType);

                var totalStatRequirement = random.Next(5, 10) + turn * 10;
                var statRequirement = HeroGameCaseStatRequirement.BuildRandom(totalStatRequirement, selectionData.MainRequirementStatType);

                var selection = new HeroGameCaseSelection(caseIndex, selectionIndex, selectionData, statReward, statRequirement);
                selections.Add(selection);
            }

            var heroCase = new HeroGameCase(caseIndex, casePool[caseIndex], selections);
            CurrentCases.Add(heroCase);
        }
    }
}
