using UnityEngine;

[CreateAssetMenu(fileName = "ChapterData", menuName = "Scriptable Objects/HeroGameChapterScriptableData")]
public class HeroGameChapterScriptableData : ScriptableObject
{
    public int Day;
    public HeroGameCaseScriptableData LastCaseData;
}
