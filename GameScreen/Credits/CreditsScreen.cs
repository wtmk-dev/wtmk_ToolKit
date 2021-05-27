using System.Collections;
using System.Collections.Generic;

public class CreditsScreen : IState
{
    public string Tag { get; }
    public IStateView View { get; }
    public IState NextState { get; }
    public IList<IState> ValidTransitions { get; set; }
    public void OnEnter() { }
    public void OnExit() { }
    public bool OnUpdate() { return false; }

    private CreditsScreenView _View;
    public CreditsScreen(CreditsScreenView view)
    {
        View = view;
        _View = (CreditsScreenView)View;
    }
}