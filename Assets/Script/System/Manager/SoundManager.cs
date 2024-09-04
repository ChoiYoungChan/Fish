using System.Collections.Generic;
using UniRx;
using UnityEngine;

public enum SeType
{
    Splash
}

public class SoundManager : SingletonClass<SoundManager>
{
    [Header("SE")]
    [SerializeField] List<AudioClip> seClips;
    [SerializeField] AudioSource seSource;
    [SerializeField] AudioSource bgmSource;

    public Subject<bool> OnSwitchedSe = new Subject<bool>();
    public Subject<bool> OnSwitchedBgm = new Subject<bool>();

    private void Start()
    {
        PlayBgm();

        OnSwitchedBgm
            .Subscribe(on => {
                if (on)
                {
                    PlayBgm();
                }
                else
                {
                    StopBGM();
                }
            })
            .AddTo(this);
    }

    public void SwitchSe()
    {
        var isOn = IsOnSe();
        PlayerPrefs.SetInt("SeIsOn", System.Convert.ToInt32(isOn));
        OnSwitchedSe.OnNext(IsOnSe());
    }

    public void SwitchBgm()
    {
        var isOn = IsOnBgm();
        PlayerPrefs.SetInt("BgmIsOn", System.Convert.ToInt32(isOn));
        OnSwitchedBgm.OnNext(IsOnBgm());
    }

    public bool IsOnSe()
    {
        return PlayerPrefs.GetInt("SeIsOn", 0) == 0;
    }

    public bool IsOnBgm()
    {
        return PlayerPrefs.GetInt("BgmIsOn", 0) == 0;
    }

    public void PlaySe(SeType type, bool loop = false)
    {
        if (!IsOnSe()) return;

        seSource.loop = loop;

        if (loop)
        {
            PlaySeWithLoop(seClips[(int)type]);
        }
        else
        {
            PlaySeOneShot(seClips[(int)type]);
        }
    }

    void PlaySeWithLoop(AudioClip clip)
    {
        if (seSource.isPlaying && seSource.clip == clip) return;
        seSource.clip = clip;
        seSource.Play();
    }

    void PlaySeOneShot(AudioClip clip)
    {
        seSource.PlayOneShot(clip);
    }

    public void StopSe(SeType type)
    {
        if (seSource.clip == seClips[(int)type] && seSource.isPlaying)
        {
            seSource.Stop();
            seSource.loop = false;
        }
    }

    public void PlayBgm()
    {
        if (!IsOnBgm() || bgmSource.isPlaying) return;
        bgmSource.Play();
    }

    public void StopBGM()
    {
        bgmSource.Stop();
    }
}
