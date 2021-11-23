using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class SoundManager
{
    public void PlayAudioClip(AudioClip aClip, float volume = 0f, bool loop = false)
    {
        CheckUpdateEffectAudioSource();

        if (_EffectAudioSource != null)
        {
            _EffectAudioSource.clip = aClip;
            _EffectAudioSource.loop = loop;
            _EffectAudioSource.volume = volume;
            _EffectAudioSource.Play();
        }
    }

    public void PlayOnMainAudio(AudioClip aClip, float volume = 1f, bool loop = false)
    {
        if (_MainAudioSource.isPlaying)
        {
            _MainAudioSource.DOFade(0, 0.3f).OnComplete(() =>
            {
                PlayMainAudio(aClip, volume, loop);
            });
        }
        else
        {
            PlayMainAudio(aClip, volume, loop);
        }
    }

    private void PlayMainAudio(AudioClip aClip, float volume = 1f, bool loop = false)
    {
        _MainAudioSource.clip = aClip;
        _MainAudioSource.loop = loop;
        _MainAudioSource.volume = volume;
        _MainAudioSource.Play();
    }

    public void PlayOneShot(AudioClip aClip, float volume = 1f)
    {
        _EffectAudioSource.PlayOneShot(aClip, volume);
    }

    public void PlayScheduled(AudioClip aClip, double delay = 0.0, float volume = 1f, bool loop = false)
    {
        /*
        double schedule = AudioSettings.dspTime + delay;

        _ScheduledAudioSources[_BufferIndex].clip = aClip;
        _ScheduledAudioSources[_BufferIndex].loop = loop;
        _ScheduledAudioSources[_BufferIndex].volume = volume;
        
        _ScheduledAudioSources[_BufferIndex].PlayScheduled(schedule);

        _BufferIndex++;

        if(_BufferIndex >= _ScheduledAudioSources.Count -1)
        {
            _BufferIndex = 0;
        }
        */
    }

    public void PlaySoundEffectDelayed(AudioClip aClip, float delay, float volume = 1f, bool loop = false)
    {
        CheckUpdateEffectAudioSource();

        if (_EffectAudioSource != null)
        {
            _EffectAudioSource.clip = aClip;
            _EffectAudioSource.loop = loop;
            _EffectAudioSource.volume = volume;
            _EffectAudioSource.PlayDelayed(delay);
        }
    }

    public double GetDurationFromClip(AudioClip aClip)
    {
        return (double)aClip.samples / aClip.frequency;
    }

    private EventManager _EventManager = EventManager.Instance;
    private AudioEvent _Event = new AudioEvent();

    private Queue<AudioSource> _AvaliableAudioSources;
    private List<AudioSource> _ScheduledAudioSources;

    private AudioClip _CurrentOneShot;

    private AudioSource _MainAudioSource;
    private AudioSource _EffectAudioSource;

    private int _ScheduledBufferSize = 6;
    private float _On = 1, _Off = 0;

    public SoundManager(List<AudioSource> audioSources)
    {
        _ScheduledBufferSize = audioSources.Count;
        _AvaliableAudioSources = new Queue<AudioSource>();
        _ScheduledAudioSources = new List<AudioSource>();

        for (int i = 0; i < audioSources.Count; i++)
        {
            _AvaliableAudioSources.Enqueue(audioSources[i]);
        }

        _MainAudioSource = _AvaliableAudioSources.Dequeue();
        _EffectAudioSource = _AvaliableAudioSources.Dequeue();

        RegisterCallbacks();
    }

    private void CheckUpdateEffectAudioSource()
    {
        if (_EffectAudioSource == null)
        {
            _EffectAudioSource = _AvaliableAudioSources.Dequeue();
        }
        else if (_EffectAudioSource.isPlaying)
        {
            _AvaliableAudioSources.Enqueue(_EffectAudioSource);
            _EffectAudioSource = _AvaliableAudioSources.Dequeue();
        }
    }

    private void RegisterCallbacks()
    {
        _EventManager.RegisterEventCallback(_Event.Play, OnPlay);
        _EventManager.RegisterEventCallback(_Event.PlayOnMain, OnPlayOnMain);
        _EventManager.RegisterEventCallback(_Event.PlayOneShot, OnPlayOneShot);

        _EventManager.RegisterEventCallback(_Event.FadeOutEffect, OnFadeOutEffect);
        _EventManager.RegisterEventCallback(_Event.FadeOutMain, OnFadeOutMain);
    }

    private void OnPlayOnMain(string name, object data)
    {
        SoundManagerPlayArgs args = (SoundManagerPlayArgs)data;
        PlayOnMainAudio(args.Aclip, args.Volume, args.Loop);
    }

    private void OnPlay(string name, object data)
    {
        SoundManagerPlayArgs args = (SoundManagerPlayArgs)data;
        PlayAudioClip(args.Aclip, args.Volume, args.Loop);
    }

    private void OnPlayOneShot(string name, object data)
    {
        _CurrentOneShot = (AudioClip)data;
        PlayAudioClip(_CurrentOneShot);
    }

    private void OnFadeOutMain(string name, object data)
    {
        if (_MainAudioSource.isPlaying)
        {
            _MainAudioSource.DOFade(_Off, 0.3f).OnComplete(() =>
            {
                _MainAudioSource.Stop();
                _MainAudioSource.DOFade(_On, 0.1f);
            });
        }
    }

    private void OnFadeOutEffect(string name, object data)
    {
        if (_EffectAudioSource.isPlaying)
        {
            _EffectAudioSource.DOFade(_Off, 0.3f).OnComplete(() =>
            {
                _EffectAudioSource.Stop();
                _EffectAudioSource.DOFade(_On, 0.1f);
            });
        }
    }
}

[Serializable]
public class SoundManagerPlayArgs
{
    public AudioClip Aclip;
    public float Volume;
    public bool Loop;
}

