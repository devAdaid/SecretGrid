using System;

[Serializable]
public struct HeroGameCaseStatReward
{
    public int Strength;
    public int Agility;
    public int Intelligence;
    public int Secret;

    public static readonly HeroGameCaseStatReward Empty = new HeroGameCaseStatReward();

    public HeroGameCaseStatReward(int strength, int agility, int intelligence, int secret = 0)
    {
        Strength = strength;
        Agility = agility;
        Intelligence = intelligence;
        Secret = secret;
    }

    public bool IsEmpty()
    {
        return Strength == 0 && Agility == 0 && Intelligence == 0 && Secret == 0;
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

        var secondValue = random.Next(1, remainingValue);
        var thirdValue = remainingValue - secondValue;

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
            result += $"{HeroGameStatType.Intelligence.ToIconString()}+{Intelligence}  ";
        }
        if (Secret > 0)
        {
            result += $"{HeroGameStatType.Secret.ToIconString()}+{Secret}  ";
        }

        return result;
    }
}
