using System.Collections.Generic;
using UnityEngine;

public class HeroGameCaseStaticData
{
    public readonly string Id;
    public readonly Sprite Sprite;
    public readonly string Title_Ko;
    public readonly string Title_En;
    public readonly string Description_Ko;
    public readonly string Description_En;
    public readonly List<HeroGameCaseSelectionStaticData> SelectionDataList;

    public HeroGameCaseStaticData(
        string id,
        Sprite sprite,
        string title_ko,
        string title_en,
        string description_ko,
        string description_en,
         List<HeroGameCaseSelectionStaticData> selectionDataList)
    {
        Id = id;
        Sprite = sprite;
        Title_Ko = title_ko;
        Title_En = title_en;
        Description_Ko = description_ko;
        Description_En = description_en;
        SelectionDataList = selectionDataList;
    }

    public static HeroGameCaseStaticData Build(HeroGameCaseScriptableData data)
    {
        var selections = new List<HeroGameCaseSelectionStaticData>();
        foreach (var select in data.Selections)
        {
            selections.Add(HeroGameCaseSelectionStaticData.Build(select));
        }

        return new HeroGameCaseStaticData(
            data.name,
            data.Sprite,
            data.Title_Ko,
            data.Title_En,
            data.Description_Ko,
            data.Description_En,
            selections
        );
    }
}
