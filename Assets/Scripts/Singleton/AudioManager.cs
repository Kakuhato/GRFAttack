using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class AudioManager : RegulatorSingleton<AudioManager>
{
    private AudioSource bgmSource;
    private AudioSource effectSource;


    public void SetBgmVolume(float volume)
    {
        bgmSource.volume = volume;
        GameDataManager.Instance.musicData.musicVolume = volume;
    }

    public void SetEffectVolume(float volume)
    {
        effectSource.volume = volume;
        GameDataManager.Instance.musicData.effectVolume = volume;
    }

    public void SetBgmMute(bool mute)
    {
        bgmSource.mute = mute;
        GameDataManager.Instance.musicData.musicOn = !mute;
    }

    public void SetEffectMute(bool mute)
    {
        effectSource.mute = mute;
        GameDataManager.Instance.musicData.effectOn = !mute;
    }

    public float GetBgmVolume()
    {
        return bgmSource.volume;
    }

    public float GetEffectVolume()
    {
        return effectSource.volume;
    }

    public bool IsBgmMute()
    {
        return bgmSource.mute;
    }

    public bool IsEffectMute()
    {
        return effectSource.mute;
    }


    public void PlaySound(String clip)
    {
    }

    public void PlayBackGroundMusic(string clipName)
    {
        AudioClip clip = Resources.Load<AudioClip>(clipName);
        if (clip == null)
        {
            Debug.LogError("Audio clip not found: " + clipName);
            return;
        }

        if (bgmSource.isPlaying)
        {
            return;
        }

        bgmSource.clip = clip;
        bgmSource.Play();
    }


    protected override void InitialSingleton()
    {
        base.InitialSingleton();
        bgmSource = gameObject.AddComponent<AudioSource>();
        bgmSource.loop = true; // Loop the background music
        effectSource = gameObject.AddComponent<AudioSource>();

        LoadSettings();
    }

    private void LoadSettings()
    {
        SetBgmMute(!GameDataManager.Instance.musicData.musicOn);
        SetBgmVolume(GameDataManager.Instance.musicData.musicVolume);
        SetEffectMute(!GameDataManager.Instance.musicData.effectOn);
        SetEffectVolume(GameDataManager.Instance.musicData.effectVolume);
    }
}