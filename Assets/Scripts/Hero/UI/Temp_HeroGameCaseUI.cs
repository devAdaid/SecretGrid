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
        statText.text = $"ÇöÀç ½ºÅÈ: " +
                    $"Èû {HeroGameContextHolder.I.GameContext.Player.Strength} " +
                    $"¹ÎÃ¸ {HeroGameContextHolder.I.GameContext.Player.Agility} " +
                    $"Áö´É {HeroGameContextHolder.I.GameContext.Player.Intelligence}";

        caseText.text = string.Empty;
        foreach (var heroCase in HeroGameContextHolder.I.GameContext.CurrentCases)
        {
            caseText.text += $"{heroCase.StaticData.Title}: {heroCase.StaticData.Description}";
            caseText.text += System.Environment.NewLine;

            for (var selectIndex = 0; selectIndex < heroCase.StaticData.SelectionDataList.Count; selectIndex++)
            {
                var selectData = heroCase.StaticData.SelectionDataList[selectIndex];
                caseText.text += $"{selectData.Description}";
                caseText.text += System.Environment.NewLine;
                caseText.text += $"  - Á¶°Ç: " +
                    $"Èû {selectData.StatRequirement.Strength} " +
                    $"¹ÎÃ¸ {selectData.StatRequirement.Agility} " +
                    $"Áö´É {selectData.StatRequirement.Intelligence}";
                caseText.text += System.Environment.NewLine;
                caseText.text += $"  - º¸»ó: " +
                    $"Èû {selectData.StatReward.Strength} " +
                    $"¹ÎÃ¸ {selectData.StatReward.Agility} " +
                    $"Áö´É {selectData.StatReward.Intelligence}";
                caseText.text += System.Environment.NewLine;
                caseText.text += $"  - ¼º°ø È®·ü: 100%";
                caseText.text += System.Environment.NewLine;
            }

            caseText.text += System.Environment.NewLine;
        }
    }
}
