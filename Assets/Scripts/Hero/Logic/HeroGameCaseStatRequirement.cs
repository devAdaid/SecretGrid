using System;
using System.Collections.Generic;

[Serializable]
public struct HeroGameCaseStatRequirement : IEquatable<HeroGameCaseStatRequirement>
{
    public int Strength;
    public int Agility;
    public int Intelligence;

    public static readonly HeroGameCaseStatRequirement None = new HeroGameCaseStatRequirement();

    public HeroGameCaseStatRequirement(int strength, int agility, int intelligence)
    {
        Strength = strength;
        Agility = agility;
        Intelligence = intelligence;
    }

    public static HeroGameCaseStatRequirement BuildRandom(int totalValue, HeroGameStatType mainStatType)
    {
        var values = GetRandomValues(totalValue);
        switch (mainStatType)
        {
            case HeroGameStatType.Strength:
                return new HeroGameCaseStatRequirement(values[0], values[1], values[2]);
            case HeroGameStatType.Agility:
                return new HeroGameCaseStatRequirement(values[2], values[0], values[1]);
            case HeroGameStatType.Intelligence:
                return new HeroGameCaseStatRequirement(values[2], values[1], values[0]);
        }

        return new HeroGameCaseStatRequirement(0, 0, 0);
    }

    private static int[] GetRandomValues(int totalValue)
    {
        Random random = new Random();

        int firstValue = (int)(totalValue * (random.Next(40, 60) / 100.0));

        int remainingValue = totalValue - firstValue;

        int secondValue = remainingValue / 2;
        int thirdValue = remainingValue - secondValue;

        return new int[] { firstValue, secondValue, thirdValue };
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

    public bool Equals(HeroGameCaseStatRequirement other)
    {
        return Strength == other.Strength
            && Agility == other.Agility
            && Intelligence == other.Intelligence;
    }

    public override bool Equals(object obj)
    {
        return base.Equals(obj);
    }

    public override int GetHashCode()
    {
        return base.GetHashCode();
    }

    public static bool operator ==(HeroGameCaseStatRequirement lhs, HeroGameCaseStatRequirement rhs) => lhs.Equals(rhs);
    public static bool operator !=(HeroGameCaseStatRequirement lhs, HeroGameCaseStatRequirement rhs) => !lhs.Equals(rhs);
}
