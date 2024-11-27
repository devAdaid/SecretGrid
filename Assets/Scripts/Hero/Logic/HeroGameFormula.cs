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

    public static int CalculateScore(bool endByEnding, int day, float playTime, HeroPlayerContext playerContext)
    {
        var score = 0;

        score += CalculateScore_End(endByEnding, day);
        score += CalculateScore_PlayTime(playTime);
        score += CalculateScore_Stat(playerContext);

        return score;
    }

    public static int CalculateScore_End(bool endByEnding, int day)
    {
        // 엔딩 도달 보너스
        if (endByEnding)
        {
            return 2000;
        }
        // 엔딩 전 게임 오버 시, 진행된 Day에 비례해 점수 가점
        else
        {
            return day * 10;
        }
    }

    public static int CalculateScore_PlayTime(float playTimeInSeconds)
    {
        float maxTime = 300f;  // 5분
        float minTime = 0f;    // 0초
        float maxScore = 2000f;  // 최대 점수
        float minScore = 1000f;  // 기본 점수 (5분에 해당)

        if (playTimeInSeconds <= maxTime && playTimeInSeconds > minTime)
        {
            // 로그 함수 적용 (편차가 적도록 하기 위해)
            float score = maxScore - (float)(Math.Log(playTimeInSeconds + 1) * (maxScore - minScore) / Math.Log(maxTime + 1));
            return (int)Math.Round(score);
        }
        else if (playTimeInSeconds == minTime)
        {
            return (int)maxScore; // 0초일 경우 2000점
        }
        else
        {
            return (int)minScore; // 5분 이상은 기본 점수 1000점
        }
    }

    public static int CalculateScore_Stat(HeroPlayerContext playerContext)
    {
        var score = 0;

        // 스탯 보너스
        score += playerContext.Strength;
        score += playerContext.Agility;
        score += playerContext.Intelligence;

        // 남은 Secret 보너스
        score += playerContext.Secret * 100;

        return score;
    }
}