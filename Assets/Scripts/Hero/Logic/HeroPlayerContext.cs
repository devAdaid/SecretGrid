using System;

public class HeroPlayerContext
{
    public int Strength { get; private set; }
    public int Agility { get; private set; }
    public int Intelligence { get; private set; }
    public int Secret { get; private set; }


    private static readonly int STAT_INITIAL = 10;
    private static readonly int SECRET_INITIAL = 3;

    public HeroPlayerContext()
    {
        Strength = STAT_INITIAL;
        Agility = STAT_INITIAL;
        Intelligence = STAT_INITIAL;
        Secret = SECRET_INITIAL;
    }

    public HeroPlayerContext(int strength, int agility, int intelligence)
    {
        Strength = strength;
        Agility = agility;
        Intelligence = intelligence;
        Secret = SECRET_INITIAL;
    }

    public void AddStatReward(HeroGameCaseStatReward reward)
    {
        Strength += reward.Strength;
        Agility += reward.Agility;
        Intelligence += reward.Intelligence;
        Secret += reward.Secret;
    }

    public void DecreaseSecret(int amount)
    {
        Secret -= amount;
        Secret = Math.Max(Secret, 0);
    }

    public int GetSuccessPercent(HeroGameCaseStatRequirement requirement)
    {
        var result = 100;
        var decreaseValueMax = 0;
        if (Strength < requirement.Strength)
        {
            var decreaseValue = (requirement.Strength - Strength) * 100 / requirement.Strength;
            decreaseValueMax = Math.Max(decreaseValueMax, decreaseValue);
        }
        if (Agility < requirement.Agility)
        {
            var decreaseValue = (requirement.Agility - Agility) * 100 / requirement.Agility;
            decreaseValueMax = Math.Max(decreaseValueMax, decreaseValue);
        }
        if (Intelligence < requirement.Intelligence)
        {
            var decreaseValue = (requirement.Intelligence - Intelligence) * 100 / requirement.Intelligence;
            decreaseValueMax = Math.Max(decreaseValueMax, decreaseValue);
        }

        result -= decreaseValueMax;

        result = Math.Max(0, result);

        return result;
    }
}