using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class HeroGameCaseSelectionScriptableData
{
    [SerializeField]
    [TextArea]
    public string Description_Ko;
    [SerializeField]
    [TextArea]
    public string Description_En;

    [SerializeField]
    public HeroGameStatType MainRewardStatType;

    [SerializeField]
    public HeroGameStatType MainRequirementStatType;
}


[CreateAssetMenu(fileName = "Case", menuName = "Scriptable Objects/HeroGameCaseScriptableData")]
public class HeroGameCaseScriptableData : ScriptableObject
{
    public Sprite Sprite;

    [TextArea]
    public string Title_Ko;
    [TextArea]
    public string Title_En;

    [TextArea]
    public string Description_Ko;
    [TextArea]
    public string Description_En;

    public List<HeroGameCaseSelectionScriptableData> Selections;
}
