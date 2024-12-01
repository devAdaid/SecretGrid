using UnityEngine;
using UnityEngine.SceneManagement;

public class TitleUI : MonoBehaviour
{
    [SerializeField]
    private HeroGameButtonBase startButton;

    [SerializeField]
    private HeroGameButtonBase leaderboardButton;

    [SerializeField]
    private HeroGameButtonBase creditsButton;

    [SerializeField]
    private HeroGameButtonBase quitButton;

    private void Awake()
    {
        startButton.AddOnClickListener(() => SceneManager.LoadScene("Hero"));
        leaderboardButton.AddOnClickListener(() => SceneManager.LoadScene("Leaderboard"));
        creditsButton.AddOnClickListener(() => Application.OpenURL("https://adaid.notion.site/The-Secret-Hero-Credit-14a84d0908ea8062ad2ee51ae73888ba?pvs=4"));
        quitButton.AddOnClickListener(OnQuitButtonClicked);
    }

    private void Start()
    {
        AudioManager.I.PlayBGM(BGMType.Title);
    }

    private void OnQuitButtonClicked()
    {
        Application.Quit();

#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }
}
