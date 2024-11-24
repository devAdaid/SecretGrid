using UnityEngine;
using UnityEngine.UIElements;

public class SettingMenuController : MonoBehaviour
{
    private UIDocument _document;
    private Slider _musicSlider;
    private Slider _soundSlider;
    private Button _saveButton;

    private void Awake()
    {
        _document = GetComponent<UIDocument>();

        _musicSlider = _document.rootVisualElement.Q<Slider>("MusicSlider");
        _musicSlider.RegisterValueChangedCallback(OnBGMVolumeChanged);

        _soundSlider = _document.rootVisualElement.Q<Slider>("SoundSlider");
        _soundSlider.RegisterValueChangedCallback(OnSFXVolumeChanged);

        _saveButton = _document.rootVisualElement.Q<Button>("SaveButton");
        _saveButton.clicked += OnSaveButtonClicked;

        _document.rootVisualElement.Q<VisualElement>("Panel").style.display = DisplayStyle.None;
    }

    private void OnBGMVolumeChanged(ChangeEvent<float> evt)
    {
        AudioManager.I.SetBGMVolume(evt.newValue);
    }

    private void OnSFXVolumeChanged(ChangeEvent<float> evt)
    {
        AudioManager.I.SetSFXVolume(evt.newValue);
    }

    private void OnSaveButtonClicked()
    {
        _document.rootVisualElement.Q<VisualElement>("Panel").style.display = DisplayStyle.None;
    }
}