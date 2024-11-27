using RedBlueGames.Tools.TextTyper;
using UnityEngine;
using UnityEngine.SceneManagement;

public class HeroGameScoreResultUI : MonoBehaviour
{
    [SerializeField]
    private GameObject root;

    [SerializeField]
    private TextTyper endingScoreText;

    [SerializeField]
    private TextTyper playTimeScoreText;

    [SerializeField]
    private TextTyper statScoreText;

    [SerializeField]
    private TextTyper totalScoreText;

    [SerializeField]
    private HeroGameButtonBase leaderBoardButton;

    [SerializeField]
    private HeroGameButtonBase titleButton;

    [SerializeField]
    private HeroGameButtonBase restartButton;

    private void Awake()
    {
        leaderBoardButton.AddOnClickListener(() => SceneManager.LoadScene("Leaderboard"));
        titleButton.AddOnClickListener(() => SceneManager.LoadScene("Title"));
        restartButton.AddOnClickListener(() => HeroGameContextHolder.I.RestartGame());
    }

    public void Show(HeroGameContext gameContext)
    {
        root.SetActive(true);

        endingScoreText.TypeText($"Ending: {HeroGameFormula.CalculateScore_End(gameContext.GameState == GameState.EndByEnding, gameContext.Day)}");
        playTimeScoreText.TypeText($"Play time: {HeroGameFormula.CalculateScore_PlayTime(gameContext.GetPlayTime())}");
        statScoreText.TypeText($"Stat: {HeroGameFormula.CalculateScore_Stat(gameContext.Player)}");
        totalScoreText.TypeText($"Total: {gameContext.GetScore()}");
    }

    public void Hide()
    {
        root.SetActive(false);
    }
}
