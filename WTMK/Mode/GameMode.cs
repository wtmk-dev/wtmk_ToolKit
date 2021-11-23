using System.Collections;
using System.Collections.Generic;
using System;
public class GameMode : State
{
    public event Action OnModeChange;
    private EventManager _EventManager = EventManager.Instance;

    protected virtual void RegisterGameScreenEvents()
    {
        _EventManager.RegisterEventCallback(GameScreenEvent.GameSelect.ToString(), OnGameSelect);
        _EventManager.RegisterEventCallback(GameScreenEvent.GameSelected.ToString(), OnGameSelected);
    }

    protected virtual void UnregisterGameScreenEvents()
    {
        _EventManager.UnregisterEventCallback(GameScreenEvent.GameSelect.ToString(), OnGameSelect);
        _EventManager.UnregisterEventCallback(GameScreenEvent.GameSelected.ToString(), OnGameSelected);
    }

    protected virtual void OnGameSelect(string name, object data)
    {
        //override to unregister inputs
    }

    protected virtual void OnGameSelected(string name, object data)
    {
        string nextGame = (string)data;

        if (Tag != nextGame)
        {
            //TO:DO check for valid transtion
            NextState = nextGame;
            _Ready = true;

            OnModeChange?.Invoke();
        }
    }

    protected virtual void OnGameSelectExit(string name, object data)
    {
        //override to register inputs
    }

}
