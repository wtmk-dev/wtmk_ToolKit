using System.Collections.Generic;
public class StateProcess 
{
    public delegate void Enter();
    public delegate void Update();

    public Dictionary<string, Enter> StateEnter { get { return _StateEnter; } }
    public Dictionary<string, Update> StateUpdate { get { return _StateUpdate; } }

    public void Clear()
    {
        _StateEnter.Clear();
        _StateUpdate.Clear();
    }

    public void RegisterEnter(string state, Enter onEnter)
    {
        _StateEnter.Add(state, onEnter);
    }

    public void RegisterUpdate(string state, Update onUpdate)
    {
        _StateUpdate.Add(state, onUpdate);
    }

    private Dictionary<string, Enter> _StateEnter = new Dictionary<string, Enter>();
    private Dictionary<string, Update> _StateUpdate = new Dictionary<string, Update>();
}