public struct HeroGameCaseStatReward
{
    public int Strength { get; private set; }
    public int Agility { get; private set; }
    public int Intelligence { get; private set; }

    public HeroGameCaseStatReward(int strength, int agility, int intelligence)
    {
        Strength = strength;
        Agility = agility;
        Intelligence = intelligence;
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
