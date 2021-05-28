using System;
using System.Collections;
using System.Collections.Generic;

public class StartScreen : IState
{
    public string Tag { get; }
    public IStateView View { get; }
    public IState NextState { get; private set; }
    public IList<IState> ValidTransitions { get; set; }
    
    public void OnEnter() 
    {
        _View.SetActive(true);
    }
    
    public void OnExit() 
    { 
    
    }

    public bool OnUpdate() 
    { 
        return _Ready; 
    }

    private StartScreenView _View;

    private EventManager _EventManager = EventManager.Instance;
    private StartScreenEvent _Event = new StartScreenEvent();

    private Dood _Dood;

    private bool _Ready = false;

    public StartScreen(StartScreenView view)
    {
        View = view;
        _View = (StartScreenView)View;

        RegisterCallBacks();
    }

    private void RegisterCallBacks()
    {
        _EventManager.RegisterEventCallback(_Event.ShowHelpScreen, TransitionToHelp);
    }

    private void TransitionToHelp(string name, object data)
    {
        HelpScreen helpScreen = null;

        for(int i = 0; i < ValidTransitions.Count; i++)
        {
            helpScreen = (HelpScreen)ValidTransitions[i];
            if(helpScreen != null)
            {
                NextState = helpScreen;
                return;
            }
        }

        if(helpScreen == null)
        {
            _Dood.Error("Help Screen is not a Valid Transition");
        }
    }
}