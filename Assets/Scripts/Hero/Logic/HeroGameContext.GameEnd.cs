using System;

public partial class HeroGameContext
{
    private void ProcessGameOver(float endTime)
    {
        gameEndTime = endTime;
        GameState = GameState.EndByGameOver;

        //TODO: 초기화 대신 결과 UI 표시 및 랭킹 기록하도록 수정
        //Initialize();
    }

    private void ProcessGameEnd(float endTime)
    {
        gameEndTime = endTime;
        GameState = GameState.EndByEnding;

        //TODO: 초기화 대신 결과 UI 표시 및 랭킹 기록하도록 수정
        //Initialize();
    }

    public float GetPlayTime()
    {
        return Math.Max(gameEndTime - gameStartTime, 0);
    }

    public int GetScore()
    {
        var playTime = Math.Max(gameEndTime - gameStartTime, 0);
        return HeroGameFormula.CalculateScore(GameState == GameState.EndByEnding, Day, playTime, Player);
    }
}