using System;
using System.Collections;
using System.Collections.Generic;

public class StartScreen : IState
{
    public string Tag { get; }
    public IStateView View { get; }
    public virtual string NextState { get; protected set; }
    public IList<string> ValidTransitions { get; set; }
    
    public virtual void OnEnter() 
    {
        _Ready = false;
        _View.SetActive(true);
    }
    
    public virtual void OnExit() 
    {
        _View.SetActive(false);
    }

    public virtual bool OnUpdate() 
    {
        return _Ready; 
    }

    private GameData _GameData = GameData.Instance;
    private GameScreenTag _ScreenTags = new GameScreenTag();
    private StartScreenView _View;
    private EventManager _EventManager = EventManager.Instance;
    private StartScreenEvent _Event = new StartScreenEvent();
    private Dood _Dood = Dood.Instance;
    protected bool _Ready = false; //When true transitions

    public StartScreen(StartScreenView view)
    {
        View = view;
        _View = (StartScreenView)View;

        RegisterCallBacks();

        Tag = _ScreenTags.Start;
    }

    protected virtual void RegisterCallBacks()
    {
        _EventManager.RegisterEventCallback(_Event.ShowHelpScreen, TransitionToHelp);
        _EventManager.RegisterEventCallback(_Event.NewGame, NewGame);
    }

    private void NewGame(string name, object data)
    {
        NextState = _ScreenTags.Game;
        _GameData.IsNewGame = true;
        _Ready = true;
    }

    private void TransitionToHelp(string name, object data)
    {
        NextState = _ScreenTags.Help;
    }
}