using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SettingUI : MonoSingleton<SettingUI>
{
    [SerializeField]
    private GameObject root;

    [SerializeField]
    private Button backButton;

    [SerializeField]
    private Slider bgmVolumeSlider;

    [SerializeField]
    private Slider sfxVolumeSlider;

    [SerializeField]
    private HeroGameButtonBase titleButton;

    [SerializeField]
    private GameObject titleConfirmUIRoot;

    [SerializeField]
    private HeroGameButtonBase titleCancelButton;

    [SerializeField]
    private HeroGameButtonBase titleConfirmButton;

    private void Awake()
    {
        backButton.onClick.AddListener(OnCloseButtonClicked);
        bgmVolumeSlider.onValueChanged.AddListener(OnBGMVolumeSliderChanged);
        sfxVolumeSlider.onValueChanged.AddListener(OnSFXVolumeSliderChanged);
        titleButton.AddOnClickListener(OnTitleButtonClicked);
        titleCancelButton.AddOnClickListener(OnTitleCancelButtonClicked);
        titleConfirmButton.AddOnClickListener(OnTitleConfirmButtonClicked);
    }

    private void OnEnable()
    {
        bgmVolumeSlider.SetValueWithoutNotify(AudioManager.I.BGMVolume);
        sfxVolumeSlider.SetValueWithoutNotify(AudioManager.I.SFXVolume);
    }

    public void Show()
    {
        root.SetActive(true);
    }

    public void Hide()
    {
        root.SetActive(false);
    }

    private void OnCloseButtonClicked()
    {
        root.SetActive(false);
        AudioManager.I.PlaySFX(SFXType.Cancel);
    }

    private void OnBGMVolumeSliderChanged(float value)
    {
        AudioManager.I.SetBGMVolume(value);
    }

    private void OnSFXVolumeSliderChanged(float value)
    {
        AudioManager.I.SetSFXVolume(value);
    }

    private void OnTitleButtonClicked()
    {
        titleConfirmUIRoot.gameObject.SetActive(true);
        AudioManager.I.PlaySFX(SFXType.Select);
    }

    private void OnTitleCancelButtonClicked()
    {
        titleConfirmUIRoot.gameObject.SetActive(false);
        AudioManager.I.PlaySFX(SFXType.Cancel);
    }

    private void OnTitleConfirmButtonClicked()
    {
        // TODO: 타이틀 씬 연결
        SceneManager.LoadScene("SceneSelector");
        AudioManager.I.PlaySFX(SFXType.Select);
    }
}
