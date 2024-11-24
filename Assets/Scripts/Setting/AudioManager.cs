using UnityEngine;

public enum BGMType
{
    EXCITING,
    Game1,
}

public enum SFXType
{
    BUTTON,
}

public class AudioManager : PersistentSingleton<AudioManager>
{
    [Header("---------- Audio Source ----------")]
    [SerializeField] AudioSource bgmSource;
    [SerializeField] AudioSource sfxSource;

    [Header("---------- Audio Clip ----------")]
    [SerializeField] AudioClip[] bgmClips;
    [SerializeField] AudioClip[] sfxClips;

    private void Start()
    {
        //instance.bgmSource.loop = true;
        //instance.PlayBGM(BGMType.EXCITING);
    }

    public void SetBGMVolume(float volume)
    {
        bgmSource.volume = volume;
    }

    public void SetSFXVolume(float volume)
    {
        sfxSource.volume = volume;
    }

    public void PlayBGM(BGMType bgm)
    {
        bgmSource.clip = bgmClips[(int)bgm];
        bgmSource.Play();
    }

    public void PlaySFX(SFXType sfx)
    {
        sfxSource.PlayOneShot(sfxClips[(int)sfx]);
    }
}
