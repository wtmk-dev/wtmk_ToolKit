using System.Collections;
using System.Collections.Generic;

public class CreditsScreen : IState<string>
{
    public string Tag { get; }
    public IStateView View { get; }
    public string NextState { get; }
    public IList<string> ValidTransitions { get; set; }
    public void OnEnter() { }
    public void OnExit() { }
    public bool OnUpdate() { return false; }

    private CreditsScreenView _View;
    private GameScreenTags _ScreenTags = new GameScreenTags();

    public CreditsScreen(CreditsScreenView view)
    {
        View = view;
        _View = (CreditsScreenView)View;

        Tag = _ScreenTags.Credits;
    }
}