using System;

public static class HeroGameFormula
{

    public static int GetRandomStatRewardTotalCount(int day, int upVariationValue)
    {
        var random = new Random();

        // TODO: 데이터화
        if (day < 5)
        {
            return random.Next(-3, 3) + 15 + upVariationValue * 3;
        }
        else if (day < 10)
        {
            return random.Next(-5, 5) + 25 + upVariationValue * 5;
        }
        else if (day < 17)
        {
            return random.Next(-5, 5) + 40 + upVariationValue * 8;
        }
        else
        {
            return random.Next(-10, 10) + 70 + upVariationValue * 10;
        }
    }

    public static int GetRandomStatRequirementTotalCount(int day, int upVariationValue)
    {
        var random = new Random();

        // TODO: 데이터화
        var countBase = GetRandomStatRequirementTotalCountBase(day);
        if (day < 5)
        {
            return random.Next(-2, 2) + countBase + upVariationValue * 5;
        }
        else if (day < 10)
        {
            return random.Next(-10, 10) + countBase + upVariationValue * 10;
        }
        else if (day < 17)
        {
            return random.Next(-10, 10) + countBase + upVariationValue * 15;
        }
        else
        {
            return random.Next(-15, 15) + countBase + upVariationValue * 20;
        }
    }

    public static int GetRandomStatRequirementTotalCountBase(int day)
    {
        var result = 10;
        result += day * 8;

        if (day > 5)
        {
            result += 45;
            result += (day - 5) * 10;
        }

        if (day > 10)
        {
            result += 50;
            result += (day - 10) * 15;
        }

        if (day > 17)
        {
            result += 80;
            result += (day - 17) * 30;
        }

        return result;
    }

    public static int GetRandomCasePickCount(int day)
    {
        // TODO: 데이터화
        if (day < 10)
        {
            return 2;
        }
        return 3;
    }
}