public enum HeroGameStatType
{
    None = 0,

    Strength,
    Agility,
    Intelligence,
    Secret,
}

public static class HeroGameStatTypeExtension
{
    public static string ToIconString(this HeroGameStatType type)
    {
        switch (type)
        {
            case HeroGameStatType.Secret:
                return $"<sprite=0/>";
            case HeroGameStatType.Strength:
                return $"<sprite=1/>";
            case HeroGameStatType.Agility:
                return $"<sprite=2/>";
            case HeroGameStatType.Intelligence:
                return $"<sprite=3/>";
        }
        return string.Empty;
    }

    public static string ToTooltipString(this HeroGameStatType type)
    {
        switch (type)
        {
            case HeroGameStatType.Secret:
                return $"{type.ToIconString()} Secret: If it reaches 0, game is over.";
            case HeroGameStatType.Strength:
                return $"{type.ToIconString()} Strength: How physically strong you are.";
            case HeroGameStatType.Agility:
                return $"{type.ToIconString()} Agility:  How quick and precise you are.";
            case HeroGameStatType.Intelligence:
                return $"{type.ToIconString()} Intelligence: How smart and logical you are.";
        }
        return string.Empty;
    }
}