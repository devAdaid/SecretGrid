using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "HeroGameDayData", menuName = "Scriptable Objects/HeroGameDayData")]
public class HeroGameDayData : ScriptableObject
{
    public int Day;
    public bool IsLastDay;
    public bool IsDayStartDialogueSkippable = true;
    public TextAsset DayStartDialogue;
    public bool IsDayEndDialogueSkippable = true;
    public TextAsset DayEndDialogue;
    public int MaxPhase;
    public List<HeroGameCaseScriptableData> CaseIdList;
}
