using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class SoundManager
{
    public void PlayAudioClip(AudioClip aClip, float volume = 1f, bool loop = false)
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

    public void StopAudioClip()
    {
        _EffectAudioSource.Stop();
    }

    public void PlayOneShot(AudioClip aClip, float volume = 1f)
    {
        _EffectAudioSource.PlayOneShot(aClip, volume);
    }

    public void PlayScheduled(AudioClip aClip, double delay = 0.0, float volume = 1f, bool loop = false)
    {
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

    public void FadeMainLoopTo(AudioClip aClip, float outTime, float inTime)
    {
        float volume = _MainAudioSource.volume;
        _MainAudioSource.DOFade(0, outTime).OnComplete(() =>
        {
            _MainAudioSource.Stop();

            _MainAudioSource.clip = aClip;
            _MainAudioSource.loop = true;
            _MainAudioSource.Play();

            _MainAudioSource.DOFade(volume, inTime);
        });
    }

    public void StartMainLoop()
    {
        _MainAudioSource.clip = _MainLoop[_Track];
        _MainAudioSource.loop = true;
        _MainAudioSource.volume = 0.03f;
        _MainAudioSource.Play();
    }

    public void SetMainLoopToIdelVolume()
    {
        //_MainAudioSource.DOFade(.1f, .5f);
    }

    public void SetMainLoopToPlayVolume()
    {
        //_MainAudioSource.DOFade(.5f, .3f);
    }

    public void SetMainLoopOff()
    {
        _MainAudioSource.DOFade(0f, .7f);
    }

    public double GetDurationFromClip(AudioClip aClip)
    {
        return (double)aClip.samples / aClip.frequency;
    }

    private Queue<AudioSource> _AvaliableAudioSources;
    private List<AudioSource> _ScheduledAudioSources;

    private AudioSource _MainAudioSource;
    private AudioSource _EffectAudioSource;
    private AudioSource _OneShotAudio;
    private GameObject _bzAudioSource;
    private List<AudioClip> _MainLoop;

    private int _Track = 0;

    private int _BufferIndex = 0;
    private int _ScheduledBufferSize = 6;

    private EventManager _EventManager = EventManager.Instance;

    public SoundManager(IList<AudioSource> audioSources, List<AudioClip> defaultAudio = null)
    {
        _MainLoop = defaultAudio;
        _AvaliableAudioSources = new Queue<AudioSource>();
        _ScheduledAudioSources = new List<AudioSource>();

        for (int i = 0; i < audioSources.Count; i++)
        {
            _AvaliableAudioSources.Enqueue(audioSources[i]);
        }
       
        _MainAudioSource = _AvaliableAudioSources.Dequeue();
        _EffectAudioSource = _AvaliableAudioSources.Dequeue();
        _OneShotAudio = _AvaliableAudioSources.Dequeue();

        for (int i = 0; i < _ScheduledBufferSize; i++)
        {
            _ScheduledAudioSources.Add(_AvaliableAudioSources.Dequeue());
        }

        _EventManager.RegisterEventCallback("PlayJump", OnPlayJump);
        _EventManager.RegisterEventCallback("PlayDud", OnPlayDud);
        _EventManager.RegisterEventCallback("PlayBell", OnPlayBell);
/*
        _EventManager.RegisterEventCallback(_TriggerEvent.Lever, OnPlayLever);
        _EventManager.RegisterEventCallback(_TriggerEvent.SetActive, OnPlayLever);
        _EventManager.RegisterEventCallback(_TriggerEvent.BellTriggered, OnPlayLever);
*/
    }

    //private TriggerEvent _TriggerEvent = new TriggerEvent();
    private AudioClip _Dud, _Jump, _Lever, _Footsteps, _Bell;

    public void SetSounds(AudioClip dud,AudioClip bell,AudioClip lever,AudioClip jump,AudioClip footsteps)
    {
        _Dud = dud; _Bell = bell; _Lever = lever; _Jump = jump; _Footsteps = footsteps;
    }

    private void OnPlayDud(string name, object data)
    {
        PlayAudioClip(_Dud);
    }

    private void OnPlayJump(string name, object data)
    {
        /*
        if(name == _TriggerEvent.SetActive)
        {
            (bool, int) id = ((bool, int))data;

            if(!id.Item1)
            {
                return;
            }
        }

        PlayAudioClip(_Jump);
        */
    }

    private void OnPlayLever(string name, object data)
    {
        PlayAudioClip(_Lever);
    }

    private void OnPlayBell(string name, object data)
    {
        PlayAudioClip(_Bell);
    }

    private void CheckUpdateEffectAudioSource()
    {
        if(_EffectAudioSource == null)
        {
            _EffectAudioSource = _AvaliableAudioSources.Dequeue();
        }
        else if (_EffectAudioSource.isPlaying)
        {
            _AvaliableAudioSources.Enqueue(_EffectAudioSource);
            _EffectAudioSource = _AvaliableAudioSources.Dequeue();
        }
    }

    private void SwapMainLoopClips()
    {
        _MainAudioSource.Stop();

        _Track++;

        if(_Track > _MainLoop.Count -1)
        {
            _Track = 0;
        }

        StartMainLoop();
    }
}

