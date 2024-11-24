using System;

public class HeroPlayerContext
{
    public int Strength { get; private set; }
    public int Agility { get; private set; }
    public int Intelligence { get; private set; }
    public int Secret { get; private set; }

    private static readonly int SECRET_MAX = 5;

    public HeroPlayerContext(int strength, int agility, int intelligence)
    {
        Strength = strength;
        Agility = agility;
        Intelligence = intelligence;
        Secret = SECRET_MAX;
    }

    public void AddStatReward(HeroGameCaseStatReward reward)
    {
        Strength += reward.Strength;
        Agility += reward.Agility;
        Intelligence += reward.Intelligence;
    }

    public int GetSuccessPercent(HeroGameCaseStatRequirement requirement)
    {
        var result = 100;
        if (Strength < requirement.Strength)
        {
            result -= (100 - (Strength * 100 / requirement.Strength));
        }
        if (Agility < requirement.Agility)
        {
            result -= (100 - (Agility * 100 / requirement.Agility));
        }
        if (Intelligence < requirement.Intelligence)
        {
            result -= (100 - (Intelligence * 100 / requirement.Intelligence));
        }

        result = Math.Max(0, result);

        return result;
    }
}