using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class HeroGameCaseSelectionScriptableData
{
    public HeroGameCaseSelectionType Type;

    [SerializeField]
    [TextArea]
    public string Description_Ko;
    [SerializeField]
    [TextArea]
    public string Description_En;

    public int DecreaseSecretValueOnFail = 1;

    // Random 타입일 경우
    public HeroGameStatType RandomMainRewardStatType;
    //public HeroGameStatType RandomMainRequirementStatType;

    // Fixed 타입일 경우
    public HeroGameCaseStatReward FixedStatReward;
    public HeroGameCaseStatRequirement FixedStatRequirement;
}


[CreateAssetMenu(fileName = "Case", menuName = "Scriptable Objects/HeroGameCaseScriptableData")]
public class HeroGameCaseScriptableData : ScriptableObject
{
    public int FixedDay;
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
