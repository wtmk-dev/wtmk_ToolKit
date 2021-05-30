using System;
using System.Collections;
using System.Collections.Generic;

public class StartScreen : IState
{
    public string Tag { get; }
    public IStateView View { get; }
    public string NextState { get; private set; }
    public IList<string> ValidTransitions { get; set; }
    
    public void OnEnter() 
    {
        _Ready = false;
        _View.SetActive(true);
    }
    
    public void OnExit() 
    {
        _View.SetActive(false);
    }

    public bool OnUpdate() 
    {
        return _Ready; 
    }

    private GameScreenTag _ScreenTags = new GameScreenTag();

    private StartScreenView _View;

    private EventManager _EventManager = EventManager.Instance;
    private StartScreenEvent _Event = new StartScreenEvent();

    private Dood _Dood = Dood.Instance;

    private bool _Ready = false; //When true transitions

    public StartScreen(StartScreenView view)
    {
        View = view;
        _View = (StartScreenView)View;

        RegisterCallBacks();

        Tag = _ScreenTags.Start;
    }

    private void RegisterCallBacks()
    {
        _EventManager.RegisterEventCallback(_Event.ShowHelpScreen, TransitionToHelp);
        _EventManager.RegisterEventCallback(_Event.NewGame, NewGame);
    }

    private void NewGame(string name, object data)
    {
        NextState = _ScreenTags.Game;

        _Ready = true;
    }

    private void TransitionToHelp(string name, object data)
    {
        NextState = _ScreenTags.Help;
    }
}