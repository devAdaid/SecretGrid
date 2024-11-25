using System.Collections.Generic;
using UnityEngine;

public class HeroGameCaseStaticData
{
    public readonly string Id;
    public readonly int FixedDay;
    public readonly Sprite Sprite;
    public readonly string Title_Ko;
    public readonly string Title_En;
    public readonly string Description_Ko;
    public readonly string Description_En;
    public readonly List<IHeroGameCaseSelectionStaticData> SelectionDataList;

    public HeroGameCaseStaticData(
        string id,
        int fixedDay,
        Sprite sprite,
        string title_ko,
        string title_en,
        string description_ko,
        string description_en,
         List<IHeroGameCaseSelectionStaticData> selectionDataList)
    {
        Id = id;
        FixedDay = fixedDay;
        Sprite = sprite;
        Title_Ko = title_ko;
        Title_En = title_en;
        Description_Ko = description_ko;
        Description_En = description_en;
        SelectionDataList = selectionDataList;
    }

    public static HeroGameCaseStaticData Build(HeroGameCaseScriptableData data)
    {
        var selections = new List<IHeroGameCaseSelectionStaticData>();
        foreach (var select in data.Selections)
        {
            switch (select.Type)
            {
                case HeroGameCaseSelectionType.Random:
                    selections.Add(HeroGameCaseRandomSelectionStaticData.Build(select));
                    break;
                case HeroGameCaseSelectionType.Fixed:
                    selections.Add(HeroGameCaseFixedSelectionStaticData.Build(select));
                    break;

            }
        }

        return new HeroGameCaseStaticData(
            data.name,
            data.FixedDay,
            data.Sprite,
            data.Title_Ko,
            data.Title_En,
            data.Description_Ko,
            data.Description_En,
            selections
        );
    }
}
