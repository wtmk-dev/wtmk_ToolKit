using System.Collections;
using System.Collections.Generic;

public class StartScreen : IState
{
    public string Tag { get; }
    public IStateView View { get; }
    public IState NextState { get; }
    public IList<IState> ValidTransitions { get; set; }
    public void OnEnter() { }
    public void OnExit() { }
    public bool OnUpdate() { return false; }


    private StartScreenView _View;
    public StartScreen(StartScreenView view)
    {
        View = view;
        _View = (StartScreenView)View;
    }
}