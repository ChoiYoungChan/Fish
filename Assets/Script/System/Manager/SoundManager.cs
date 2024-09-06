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

    private void Start() => PlayBgm();

    private void PlaySeWithLoop(AudioClip clip)
    {
        if (seSource.isPlaying && seSource.clip == clip) return;
        seSource.clip = clip;
        seSource.Play();
    }

    private void PlaySeOneShot(AudioClip clip)
    {
        seSource.PlayOneShot(clip);
    }

    public void SwitchSe()
    {
        var isOn = IsOnSe();
        PlayerPrefs.SetInt("SeIsOn", System.Convert.ToInt32(isOn));
    }

    public void SwitchBgm()
    {
        var isOn = IsOnBgm();
        PlayerPrefs.SetInt("BgmIsOn", System.Convert.ToInt32(isOn));
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
}
