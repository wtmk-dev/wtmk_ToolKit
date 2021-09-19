using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    

    public void PlayOnLoop(AudioClip aClip, float volume)
    {
        _MainAudioSource.clip = aClip;
        _MainAudioSource.loop = true;
        _MainAudioSource.volume = volume;
        _MainAudioSource.Play();
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

     
        _EventManager.RegisterEventCallback("PlayOneShot", OnPlayOneShot);
        _EventManager.RegisterEventCallback("PlayBGM", OnPlayBGM);
    }

    //private TriggerEvent _TriggerEvent = new TriggerEvent();
    private AudioClip _Dud, _Jump, _Lever, _Footsteps, _Bell;

    public void SetSounds(AudioClip dud,AudioClip bell,AudioClip lever,AudioClip jump,AudioClip footsteps)
    {
        _Dud = dud; _Bell = bell; _Lever = lever; _Jump = jump; _Footsteps = footsteps;
    }

    private void OnPlayOneShot(string name, object data)
    {
        AudioClip clip = (AudioClip)data;
        PlayAudioClip(clip);
    }

    private void OnPlayBGM(string name, object data)
    {
        AudioClip clip = (AudioClip)data;
        PlayOnLoop(clip, 0.1f);
        
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
}

