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
        //TODO: 성공 확률을 계산한다
        return 100;
    }
}