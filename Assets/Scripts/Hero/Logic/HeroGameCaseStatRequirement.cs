using System;
using System.Collections.Generic;

[Serializable]
public struct HeroGameCaseStatRequirement
{
    public int Strength { get; private set; }
    public int Agility { get; private set; }
    public int Intelligence { get; private set; }

    public HeroGameCaseStatRequirement(int strength, int agility, int intelligence)
    {
        Strength = strength;
        Agility = agility;
        Intelligence = intelligence;
    }

    public string ToCompareString(HeroPlayerContext playerContext)
    {
        var results = new List<string>();
        if (Strength > 0)
        {
            var satisfly = playerContext.Strength >= Strength;
            results.Add($"{HeroGameStatType.Strength.ToIconString()} {WrapBySatisfy(satisfly, playerContext.Strength.ToString())} >= {Strength}");
        }
        if (Agility > 0)
        {
            var satisfly = playerContext.Agility >= Agility;
            results.Add($"{HeroGameStatType.Agility.ToIconString()} {WrapBySatisfy(satisfly, playerContext.Agility.ToString())} >= {Agility}");
        }
        if (Intelligence > 0)
        {
            var satisfly = playerContext.Intelligence >= Intelligence;
            results.Add($"{HeroGameStatType.Intelligence.ToIconString()} {WrapBySatisfy(satisfly, playerContext.Intelligence.ToString())} >= {Intelligence}");
        }
        return string.Join("\n", results.ToArray());
    }

    private string WrapBySatisfy(bool satisfy, string text)
    {
        if (!satisfy)
        {
            text = $"<color=#579c9a>{text}</color>";
        }
        return text;
    }
}
