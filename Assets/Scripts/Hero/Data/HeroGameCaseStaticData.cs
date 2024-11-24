using System.Collections.Generic;

public class HeroGameCaseStaticData
{
    public readonly string Id;
    public readonly string Title;
    public readonly string Description;
    public readonly List<HeroGameCaseSelectionStaticData> SelectionDataList;

    public HeroGameCaseStaticData(string id, string title, string description,
         List<HeroGameCaseSelectionStaticData> selectionDataList)
    {
        Id = id;
        Title = title;
        Description = description;
        SelectionDataList = selectionDataList;
    }
}
