using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HelpScreen : IState
{
    public string Tag { get; }
    public IStateView View { get; }
    public string NextState { get; }
    public IList<string> ValidTransitions { get; set; }
    public void OnEnter() { }
    public void OnExit() { }
    public bool OnUpdate() { return false; }

    private HelpScreenView _View;
    private GameScreenTags _ScreenTags = new GameScreenTags();

    public HelpScreen(HelpScreenView view)
    {
        View = view;
        _View = (HelpScreenView)View;

        Tag = _ScreenTags.Help;
    }
}