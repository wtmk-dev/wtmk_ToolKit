using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class GameScreen : IState
{
    public string Tag { get; }
    public IStateView View { get; }
    public string NextState { get; }
    public IList<string> ValidTransitions { get; set; }

    public void OnEnter() 
    {
        Debug.Log("Game Screen Enter");
        StateEnter(); // implement in your own partial
    }

    public void OnExit() 
    {
        StateExit(); // implement in your own partial
    }

    public bool OnUpdate() 
    { 
        return StateUpdate(); 
    }
    
    private GameScreenView _View;
    private GameScreenTag _ScreenTags = new GameScreenTag();
    private GameData _GameData = GameData.Instance;
    public GameScreen(GameScreenView view)
    {
        View = view;
        _View = (GameScreenView)View;

        Tag = _ScreenTags.Game;
    }
}