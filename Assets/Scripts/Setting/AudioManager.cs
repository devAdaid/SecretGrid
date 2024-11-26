using System;
using System.Collections.Generic;
using UnityEngine;

public enum BGMType
{
    EXCITING,
    Game1,
    Game2,
}

public enum SFXType
{
    BUTTON,
    Select,
    Cancel,
    Success,
    Fail,
    Wait,
}

[Serializable]
public class BGMClipInfo
{
    public BGMType BGMType;
    public AudioClip AudioClip;
}

[Serializable]
public class SFXClipInfo
{
    public SFXType SFXType;
    public AudioClip AudioClip;
}

public class AudioManager : PersistentSingleton<AudioManager>
{
    public float BGMVolume => bgmSource.volume;
    public float SFXVolume => sfxSource.volume;

    [Header("---------- Audio Source ----------")]
    [SerializeField] AudioSource bgmSource;
    [SerializeField] AudioSource sfxSource;

    [Header("---------- Audio Clip ----------")]
    [SerializeField] BGMClipInfo[] bgmClips;
    [SerializeField] SFXClipInfo[] sfxClips;

    private Dictionary<BGMType, AudioClip> bgmMap = new();
    private Dictionary<SFXType, AudioClip> sfxMap = new();

    private readonly string KEY_VOLUME_BGM = "Volume_BGM";
    private readonly string KEY_VOLUME_SFX = "Volume_SFX";

    protected override void Awake()
    {
        base.Awake();

        foreach (var bgm in bgmClips)
        {
            bgmMap.Add(bgm.BGMType, bgm.AudioClip);
        }

        foreach (var sfx in sfxClips)
        {
            sfxMap.Add(sfx.SFXType, sfx.AudioClip);
        }

        bgmSource.volume = PlayerPrefs.GetFloat(KEY_VOLUME_BGM, 0.5f);
        sfxSource.volume = PlayerPrefs.GetFloat(KEY_VOLUME_SFX, 0.5f);
    }

    public void SetBGMVolume(float volume)
    {
        bgmSource.volume = volume;
        PlayerPrefs.SetFloat(KEY_VOLUME_BGM, volume);
    }

    public void SetSFXVolume(float volume)
    {
        sfxSource.volume = volume;
        PlayerPrefs.SetFloat(KEY_VOLUME_SFX, volume);
    }

    public void PlayBGM(BGMType bgm)
    {
        bgmSource.clip = bgmMap[bgm];
        bgmSource.Play();
    }

    public void StopBGM()
    {
        bgmSource.Stop();
    }

    public void PlaySFX(SFXType sfx)
    {
        sfxSource.PlayOneShot(sfxMap[sfx]);
    }
}
