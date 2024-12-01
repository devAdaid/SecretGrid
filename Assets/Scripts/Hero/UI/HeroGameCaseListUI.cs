using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class HeroGameCaseListUI : MonoBehaviour
{
    [SerializeField]
    private TMP_Text specialCaseText;

    [SerializeField]
    private List<HeroGameCaseItemUIControl> caseItems = new();

    public void Apply(HeroGameCaseUIData data)
    {
        for (var i = 0; i < data.CaseDataList.Count; i++)
        {
            caseItems[i].gameObject.SetActive(true);
            caseItems[i].Apply(data.CaseDataList[i]);
        }

        for (var i = data.CaseDataList.Count; i < caseItems.Count; i++)
        {
            caseItems[i].gameObject.SetActive(false);
        }

        if (data.MaxSpecialCaseCount > 1)
        {
            specialCaseText.text =
                (CommonSingleton.I.IsKoreanLanguage ? "모든 임무를 수행하세요." : "Complete all missions.")
                + $" ({data.CompletedSpecialCaseCount}/{data.MaxSpecialCaseCount})";
        }
        else
        {
            specialCaseText.text = string.Empty;
        }
    }
}
