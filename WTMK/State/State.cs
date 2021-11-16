using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class State : IState
{
    public virtual string Tag { get; protected set; }
    public virtual string NextState { get; protected set; }
    public virtual IList<string> ValidTransitions { get; set; }
    public virtual IStateView View { get; }
    public virtual void OnEnter() {}
    public virtual void OnExit()  {}
    public virtual bool OnUpdate() { return _Ready; }

    protected bool _Ready;    
}
