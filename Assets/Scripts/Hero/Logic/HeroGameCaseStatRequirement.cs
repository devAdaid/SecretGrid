using System;

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
}
