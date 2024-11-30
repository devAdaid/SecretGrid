using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "HeroGameDayData", menuName = "Scriptable Objects/HeroGameDayData")]
public class HeroGameDayData : ScriptableObject
{
    public int Day;
    public TextAsset DayStartDialogue;
    public TextAsset DayEndDialogue;
    public List<string> CaseIdList;
}
