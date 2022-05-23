using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class State<T> : IState<T>
{
    public virtual T Tag { get; protected set; }
    public virtual T NextState { get; protected set; }
    public virtual IList<T> ValidTransitions { get; protected set; }
    public virtual IStateView View { get; }
    public virtual void OnEnter() {}
    public virtual void OnExit()  {}
    public virtual bool OnUpdate() { return _Ready; }

    protected bool _Ready;
    
    public State(T tag, IList<T> validTransitions)
    {
        Tag = tag;
        ValidTransitions = validTransitions;
    }

    public State()
    {

    }
}
