using System;

[Serializable]
public struct HeroGameCaseStatReward
{
    public int Strength;
    public int Agility;
    public int Intelligence;

    public HeroGameCaseStatReward(int strength, int agility, int intelligence)
    {
        Strength = strength;
        Agility = agility;
        Intelligence = intelligence;
    }

    public static HeroGameCaseStatReward BuildRandom(int totalValue, HeroGameStatType mainStatType)
    {
        var values = GetRandomValues(totalValue);
        switch (mainStatType)
        {
            case HeroGameStatType.Strength:
                return new HeroGameCaseStatReward(values[0], values[1], values[2]);
            case HeroGameStatType.Agility:
                return new HeroGameCaseStatReward(values[2], values[0], values[1]);
            case HeroGameStatType.Intelligence:
                return new HeroGameCaseStatReward(values[2], values[1], values[0]);
        }

        return new HeroGameCaseStatReward(0, 0, 0);
    }

    private static int[] GetRandomValues(int totalValue)
    {
        Random random = new Random();

        int firstValue = (int)(totalValue * (random.Next(60, 71) / 100.0));

        int remainingValue = totalValue - firstValue;

        int secondValue = random.Next(1, remainingValue);
        int thirdValue = remainingValue - secondValue;

        return new int[] { firstValue, secondValue, thirdValue };
    }

    public string ToUIString()
    {
        var result = string.Empty;
        if (Strength > 0)
        {
            result += $"{HeroGameStatType.Strength.ToIconString()}+{Strength}  ";
        }
        if (Agility > 0)
        {
            result += $"{HeroGameStatType.Agility.ToIconString()}+{Agility}  ";
        }
        if (Intelligence > 0)
        {
            result += $"{HeroGameStatType.Intelligence.ToIconString()} +{Intelligence}  ";
        }

        return result;
    }
}
