using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameScreen : IState
{
    public string Tag { get; }
    public IStateView View { get; }
    public virtual string NextState { get; protected set; }
    public IList<string> ValidTransitions { get; set; }

    public virtual void OnEnter() 
    {
        //Debug.Log("Game Screen Enter");
        StateEnter(); // implement in your own override
    }

    public virtual void OnExit() 
    {
        StateExit(); // implement in your own override
    }

    public virtual bool OnUpdate() 
    { 
        return StateUpdate(); 
    }
    
    private GameScreenView _View;
    private GameScreenTags _ScreenTags = new GameScreenTags();
    private GameData _GameData = GameData.Instance;
    public GameScreen(GameScreenView view)
    {
        View = view;
        _View = (GameScreenView)View;

        Tag = _ScreenTags.Game;
    }

    protected virtual void StateEnter()
    {
        if (_GameData.IsNewGame)
        {
            NewGame();
        }
    }

    protected virtual void StateExit()
    {
        Debug.Log("Game Screen Exit");
    }

    protected virtual bool StateUpdate()
    {
        return false;
    }

    protected virtual void NewGame()
    {
        Debug.Log("New Game");
        _View.SetActive(true);
    }

    protected virtual void LoadGame()
    {
        Debug.Log("Load Game");
        _View.SetActive(true);
    }
}


public enum GameScreenEvent
{
    GameSelect,
    GameSelected,
    GameSelectExit,
}