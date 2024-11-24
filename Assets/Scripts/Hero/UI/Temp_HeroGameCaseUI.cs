using TMPro;
using UnityEngine;

public class Temp_HeroGameCaseUI : MonoBehaviour
{
    [SerializeField]
    private TMP_Text statText;

    [SerializeField]
    private TMP_Text caseText;

    private void Start()
    {
        ApplyUI();
    }

    public void Select11()
    {
        HeroGameContextHolder.I.GameContext.SelectAndProcess(0, 0);
        ApplyUI();
    }

    public void Select12()
    {
        HeroGameContextHolder.I.GameContext.SelectAndProcess(0, 1);
        ApplyUI();
    }

    public void Select21()
    {
        HeroGameContextHolder.I.GameContext.SelectAndProcess(1, 0);
        ApplyUI();
    }

    public void Select22()
    {
        HeroGameContextHolder.I.GameContext.SelectAndProcess(1, 1);
        ApplyUI();
    }

    private void ApplyUI()
    {
        statText.text = $"현재 스탯: " +
                    $"힘 {HeroGameContextHolder.I.GameContext.Player.Strength} " +
                    $"민첩 {HeroGameContextHolder.I.GameContext.Player.Agility} " +
                    $"지능 {HeroGameContextHolder.I.GameContext.Player.Intelligence}";

        caseText.text = string.Empty;
        foreach (var heroCase in HeroGameContextHolder.I.GameContext.CurrentCases)
        {
            caseText.text += $"{heroCase.StaticData.Title_En}: {heroCase.StaticData.Description_En}";
            caseText.text += System.Environment.NewLine;

            for (var selectIndex = 0; selectIndex < heroCase.StaticData.SelectionDataList.Count; selectIndex++)
            {
                var selectData = heroCase.Selections[selectIndex];
                caseText.text += $"{selectData.StaticData.Description_En}";
                caseText.text += System.Environment.NewLine;
                caseText.text += $"  - 조건: " +
                    $"힘 {selectData.StatRequirement.Strength} " +
                    $"민첩 {selectData.StatRequirement.Agility} " +
                    $"지능 {selectData.StatRequirement.Intelligence}";
                caseText.text += System.Environment.NewLine;
                caseText.text += $"  - 보상: " +
                    $"힘 {selectData.StatReward.Strength} " +
                    $"민첩 {selectData.StatReward.Agility} " +
                    $"지능 {selectData.StatReward.Intelligence}";
                caseText.text += System.Environment.NewLine;
                caseText.text += $"  - 성공 확률: 100%";
                caseText.text += System.Environment.NewLine;
            }

            caseText.text += System.Environment.NewLine;
        }
    }
}
