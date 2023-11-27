public class BPMTimer : Timer
{
    public int BPM 
    { 
        get { return _BPM; }
        set
        {
            _BPM = value;
            UpdateInterval(_BPM);
        }
    }

    public BPMTimer()
    {
        BPM = 90;
    }

    public BPMTimer(int bpm)
    {
        BPM = bpm;
    }

    private void UpdateInterval(int bpm)
    {
        _TimerLength = (int)(60000f / bpm);
    }

    protected int _BPM;
}
