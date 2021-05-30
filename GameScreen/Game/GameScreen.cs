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
        if(_GameData.IsNewGame)
        {
            NewGame(); // implement in your own partial
        }
    }
    public void OnExit() { }
    public bool OnUpdate() { return false; }

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