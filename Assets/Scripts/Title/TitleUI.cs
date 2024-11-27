using UnityEngine;
using UnityEngine.SceneManagement;

public class TitleUI : MonoBehaviour
{
    [SerializeField]
    private HeroGameButtonBase startButton;

    [SerializeField]
    private HeroGameButtonBase leaderboardButton;

    [SerializeField]
    private HeroGameButtonBase quitButton;

    private void Awake()
    {
        startButton.AddOnClickListener(() => SceneManager.LoadScene("Hero"));
        leaderboardButton.AddOnClickListener(() => SceneManager.LoadScene("Leaderboard"));
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
