using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HelpScreen : IState
{
    public string Tag { get; }
    public IStateView View { get; }
    public IState NextState { get; }
    public IList<IState> ValidTransitions { get; set; }
    public void OnEnter() { }
    public void OnExit() { }
    public bool OnUpdate() { return false; }

    private HelpScreenView _View;
    public HelpScreen(HelpScreenView view)
    {
        View = view;
        _View = (HelpScreenView)View;
    }
}