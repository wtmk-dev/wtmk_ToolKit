using System;
using System.Collections;
using System.Collections.Generic;

public class Timer
{
    public event Action OnTimerComplete;
    public event Action<float> OnTimerTick; //_TimeRemaning

    public bool IsTicking { get; private set; }

    public void Tick()
    {
        if(!IsTicking)
        {
            return;
        }

        if(_Timer.ElapsedMilliseconds >= _TimerLength)
        {
            Stop();
            OnTimerComplete?.Invoke();
        }
    }

    public void Start()
    {
        IsTicking = true;
        _Timer.Reset();
        _Timer.Start();
    }

    public void Start(float lenghtOfTimeInMilli)
    {
        _TimerLength = lenghtOfTimeInMilli;
        IsTicking = true;
        _Timer.Reset();
        _Timer.Start();
    }

    public void Stop()
    {
        IsTicking = false;
        _Timer.Stop();
    }

    private System.Diagnostics.Stopwatch _Timer;
    private float _TimerLength;

    public Timer(float lenghtOfTimeInMilli)
    {
        _Timer = new System.Diagnostics.Stopwatch();
        _TimerLength = lenghtOfTimeInMilli;
    }

}
