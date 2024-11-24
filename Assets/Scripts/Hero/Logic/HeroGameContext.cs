
using System;
using System.Collections.Generic;

public class HeroGameContext
{
    public HeroPlayerContext Player { get; private set; }

    public List<HeroGameCase> CurrentCases;

    private List<HeroGameCaseStaticData> casePool;

    // TODO: 챕터, 턴마다 난이도 밸런싱
    private int turn;

    public HeroGameContext()
    {
        Initialize();
    }

    private void Initialize()
    {
        Player = new HeroPlayerContext(10, 10, 10);
        CurrentCases = new List<HeroGameCase>();

        casePool = new List<HeroGameCaseStaticData>();
        foreach (var caseData in StaticDataHolder.I.GetCaseList())
        {
            casePool.Add(caseData);
        }

        turn = 0;
        AudioManager.I.PlayBGM(BGMType.Game1);

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
            AudioManager.I.PlaySFX(SFXType.Success);
        }
        else
        {
            // 사건 실패
            Player.DecreaseSecret(1);
            AudioManager.I.PlaySFX(SFXType.Fail);

            // 게임 오버
            if (Player.Secret == 0)
            {
                Initialize();
            }
        }

        turn += 1;

        if (turn == 5)
        {
            AudioManager.I.PlayBGM(BGMType.Game2);
        }

        //TODO: 게임오버, 챕터 처리
        // 다음 사건들을 구성한다.
        PickCases();
    }

    private void PickCases()
    {
        CurrentCases.Clear();

        casePool.Shuffle();

        //TODO: 챕터 구현
        var pickCount = turn < 5 ? 2 : 3;

        var random = new Random();
        for (var caseIndex = 0; caseIndex < pickCount; caseIndex++)
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
