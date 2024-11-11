using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class SettingMenuController : MonoBehaviour
{
    public AudioSource musicSource;
    public AudioSource[] soundSources;
    private UIDocument _document;
    private Slider _musicSlider;
    private Slider _soundSlider;
    private Button _saveButton;

    private void Awake()
    {
        _document = GetComponent<UIDocument>();

        _musicSlider = _document.rootVisualElement.Q<Slider>("MusicSlider");
        _musicSlider.RegisterValueChangedCallback(OnMusicVolumeChanged);

        _soundSlider = _document.rootVisualElement.Q<Slider>("SoundSlider");
        _soundSlider.RegisterValueChangedCallback(OnSoundVolumeChanged);

        _saveButton = _document.rootVisualElement.Q<Button>("SaveButton");
        _saveButton.clicked += OnSaveButtonClicked;
    }

    private void OnMusicVolumeChanged(ChangeEvent<float> evt)
    {
        musicSource.volume = evt.newValue;
    }

    private void OnSoundVolumeChanged(ChangeEvent<float> evt)
    {
        foreach (var sound in soundSources)
        {
            sound.volume = evt.newValue;
        }
    }

    private void OnSaveButtonClicked()
    {
        _document.rootVisualElement.Q<VisualElement>("Panel").style.display = DisplayStyle.None;
    }
}
