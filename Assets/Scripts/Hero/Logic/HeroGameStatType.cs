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
        if (CommonSingleton.I.IsKoreanLanguage)
        {
            switch (type)
            {
                case HeroGameStatType.Secret:
                    return $"{type.ToIconString()} 비밀: 0이 되면 게임 오버됩니다.";
                case HeroGameStatType.Strength:
                    return $"{type.ToIconString()} 힘: 당신이 얼마나 물리적으로 강한지를 나타냅니다.";
                case HeroGameStatType.Agility:
                    return $"{type.ToIconString()} 민첩: 당신이 얼마나 빠르고 정확한지를 나타냅니다.";
                case HeroGameStatType.Intelligence:
                    return $"{type.ToIconString()} 지능: 당신이 얼마나 똑똑하고 논리적인지를 나타냅니다.";
            }
        }
        else
        {
            switch (type)
            {
                case HeroGameStatType.Secret:
                    return $"{type.ToIconString()} Secret: If it reaches 0, game is over.";
                case HeroGameStatType.Strength:
                    return $"{type.ToIconString()} Strength: How physically strong you are.";
                case HeroGameStatType.Agility:
                    return $"{type.ToIconString()} Agility: How quick and precise you are.";
                case HeroGameStatType.Intelligence:
                    return $"{type.ToIconString()} Intelligence: How smart and logical you are.";
            }
        }
        return string.Empty;
    }
}