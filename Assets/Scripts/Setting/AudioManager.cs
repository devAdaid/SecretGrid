using System;
using System.Collections.Generic;
using UnityEngine;

public enum BGMType
{
    Invalid,

    Title = 10,

    Dialogue1 = 15,
    Dialogue_Tension,
    Dialogue_Xeros,

    Game1 = 20,
    Game2,
    Game3,
    Game4,
    Game5,

    Boss1 = 30,
    Boss2,

    GameOver = 50,

    End1 = 60,
    End2,
    End3,
}

public enum SFXType
{
    Invalid,
    Select,
    Cancel,
    Success,
    Fail,
    Wait,
    Type,
    Secret,
    Noise,
    Explosion,
    Disconnect,
    Alarm,
    Phone,
    Damage,
    Sfx1,
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
    [SerializeField] AudioSource typeSfxSource;

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

        bgmSource.volume = Jammer.PlayerPrefs.GetFloat(KEY_VOLUME_BGM, 0.5f);
        sfxSource.volume = Jammer.PlayerPrefs.GetFloat(KEY_VOLUME_SFX, 0.5f);
        typeSfxSource.volume = Jammer.PlayerPrefs.GetFloat(KEY_VOLUME_SFX, 0.5f);
    }

    public void SetBGMVolume(float volume)
    {
        bgmSource.volume = volume;
        Jammer.PlayerPrefs.SetFloat(KEY_VOLUME_BGM, volume);
    }

    public void SetSFXVolume(float volume)
    {
        sfxSource.volume = volume;
        typeSfxSource.volume = volume;
        Jammer.PlayerPrefs.SetFloat(KEY_VOLUME_SFX, volume);
    }

    public void PlayBGM(BGMType bgm, bool loop = true)
    {
        var clip = bgmMap[bgm];
        if (bgmSource.clip == clip)
        {
            return;
        }

        bgmSource.loop = loop;
        bgmSource.clip = clip;
        bgmSource.Play();
    }

    public void StopBGM()
    {
        bgmSource.Stop();
    }

    public void PauseBGM()
    {
        bgmSource.Pause();
    }

    public void ResumeBGM()
    {
        bgmSource.Play();
    }

    public void PlaySFX(SFXType sfx)
    {
        sfxSource.PlayOneShot(sfxMap[sfx]);
    }

    public void PlayTypeSFX(SFXType sfx)
    {
        typeSfxSource.clip = sfxMap[sfx];
        typeSfxSource.Play();
    }
}
