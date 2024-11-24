using System.Collections.Generic;
using UnityEngine;

public class HeroGameCaseListUI : MonoBehaviour
{
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
    }
}
