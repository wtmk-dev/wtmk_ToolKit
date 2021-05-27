using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Main : MonoBehaviour
{
    [SerializeField]
    private StartScreenView _ScreenView;
    [SerializeField]
    private GameScreenView _GameScreenView;
    [SerializeField]
    private HelpScreenView _HelpScreenView;
    [SerializeField]
    private CreditsScreenView _CreditsView;

    private IStateDirector[] _Directors = new IStateDirector[1];
    private GameScreenDirector _GameStateDirector;

    void Awake()
    {
        BuildGameScreens();
    }

    void Update()
    {
        for(int elm = 0; elm < _Directors.Length; elm++)
        {
            _Directors[elm].OnUpdate();
        }
    }

    private void BuildGameScreens()
    {
        StartScreen startScreen        = new StartScreen(_ScreenView);
        GameScreen gameScreen          = new GameScreen(_GameScreenView);
        HelpScreen helpScreen          = new HelpScreen(_HelpScreenView);
        CreditsScreen creditsScreen    = new CreditsScreen(_CreditsView);

        startScreen.ValidTransitions   = new IState[] { gameScreen, helpScreen, creditsScreen };
        gameScreen.ValidTransitions    = new IState[] { startScreen, helpScreen, creditsScreen };
        helpScreen.ValidTransitions    = new IState[] { startScreen, gameScreen };
        creditsScreen.ValidTransitions = new IState[] { startScreen, gameScreen };

        var gamestates                 = new IState[] { startScreen , gameScreen , helpScreen, creditsScreen };

        _GameStateDirector = new GameScreenDirector(gamestates);

        _Directors[0] = _GameStateDirector;
    }
}
