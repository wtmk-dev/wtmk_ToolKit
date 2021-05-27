using System.Collections;
using System.Collections.Generic;

public class GameScreenDirector : IStateDirector
{
    public void OnUpdate()
    {
        if(!_IsActive)
        {
            return;
        }

        for (int i = 0; i < _States.Length; i++)
        {
            _States[i].OnUpdate();
        }
    }

    private bool _IsActive;
    private IState[] _States;
    public GameScreenDirector(IState[] states)
    {
        _States = states;

        HideAllScreens();
    }

    private void HideAllScreens()
    {
        for (int i = 0; i < _States.Length; i++)
        {
            _States[i].View.SetActive(false);
        }
    }
}
