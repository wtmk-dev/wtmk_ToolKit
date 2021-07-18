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

        if (_Timer.ElapsedMilliseconds != _PreviousTimeDiff)
        {
            _DiffTimeCalc = _Timer.ElapsedMilliseconds - _PreviousTimeDiff;
            if (_DiffTimeCalc > _TickSpeed)
            {
                _PreviousTimeDiff = _Timer.ElapsedMilliseconds;
                _TimeRemaning -= _TickSpeed;

                OnTimerTick?.Invoke(_TimeRemaning);
            }
        }

        if(_TimeRemaning <= 0f)
        {
            OnTimerComplete?.Invoke();
        }
    }

    public void StartTimer()
    {
        IsTicking = true;
        _Timer.Reset();
        _Timer.Start();
    }

    public void StopTimer()
    {
        IsTicking = false;
        _Timer.Stop();
    }

    public void Reset()
    {
        IsTicking = false;
        _TimeRemaning = _TimerLength;
        _PreviousTimeDiff = 0;
        _DiffTimeCalc = 0;
    }

    private System.Diagnostics.Stopwatch _Timer;
    private float _TimerLength;
    private float _DiffTimeCalc;
    private float _PreviousTimeDiff;
    private float _TickSpeed;
    private float _TimeRemaning;

    public Timer(float lenghtOfTimeInMilli, float tickSpeedInMilli)
    {
        _Timer = new System.Diagnostics.Stopwatch();
        _TimerLength = lenghtOfTimeInMilli;
        _TickSpeed = tickSpeedInMilli;
    }
}
