using UnityEngine;

public enum BGMType
{
    EXCITING,
}

public enum SFXType
{
    BUTTON,
}

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance { get; private set; }

    [Header("---------- Audio Source ----------")]
    [SerializeField] AudioSource bgmSource;
    [SerializeField] AudioSource sfxSource;

    [Header("---------- Audio Clip ----------")]
    [SerializeField] AudioClip[] bgmClips;
    [SerializeField] AudioClip[] sfxClips;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this);
        }
        else
        {
            Destroy(this);
        }
    }

    private void Start()
    {
        instance.bgmSource.loop = true;
        instance.PlayBGM(BGMType.EXCITING);
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
        bgmSource.clip = instance.bgmClips[(int)bgm];
        bgmSource.Play();
    }

    public void PlaySFX(SFXType sfx)
    {
        sfxSource.PlayOneShot(instance.sfxClips[(int)sfx]);
    }
}
